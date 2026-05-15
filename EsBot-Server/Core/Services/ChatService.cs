using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services;

public class ChatService(
    IMapper mapper,
    ISessionRepository sessionRepository,
    ILlmInterface llmInterface) : IChatService
{
    public async Task<UserSession> CreateNewSession(string externalUserId)
    {
        UserSession session = new UserSession(externalUserId);
        await sessionRepository.AddSession(session);
        return session;
    }

    public async Task<MessageResponse> AskQuestion(QuestionRequest question)
    {
        var message = mapper.Map<Message>(question);
        await sessionRepository.AddMessage(message);

        try
        {
            var answer = await llmInterface.Ask(message.Content);
            var answerMessage = mapper.Map<Message>(answer);
            answerMessage.UserSessionId = message.UserSessionId;

            await sessionRepository.AddMessage(answerMessage);
            return mapper.Map<MessageResponse>(answerMessage);
        }
        catch (Exception ex)
        {
            // Graceful error handling: Fallback response when LLM fails
            var fallbackMessage = new Message(message.UserSessionId, true,
                "The AI service is currently unavailable. Please try again later.");
            return mapper.Map<MessageResponse>(fallbackMessage);
        }
    }

    public async Task<QuizRequestResponse> RequestQuiz(CreateQuizRequest topic)
    {
        var quizRequest = mapper.Map<QuizRequest>(topic);

        try
        {
            var quiz = await llmInterface.CreateQuiz(quizRequest);
            quizRequest.QuizItems = quiz.Items.ToList();
            await sessionRepository.AddQuizRequest(quizRequest);
            return mapper.Map<QuizRequestResponse>(quizRequest);
        }
        catch (Exception ex)
        {
            throw new ServiceUnavailableException("Could not generate quiz at this time.", ex);
        }
    }

    public async Task<EvaluationResultResponse> EvaluateAnswer(AnswerEvaluationRequest request)
    {
        try
        {
            var feedback = await llmInterface.Evaluate(request.QuestionText, request.UserAnswer);
            return new EvaluationResultResponse { Feedback = feedback, IsCorrect = feedback.Contains("Correct") };
        }
        catch (Exception ex)
        {
            return new EvaluationResultResponse { Feedback = "Unable to evaluate your answer right now.", IsCorrect = false };
        }
    }

    public async Task<IEnumerable<MessageResponse>> GetSession(Guid sessionId)
    {
        var userSession = await sessionRepository.GetSession(sessionId);
        if (userSession == null || !userSession.Messages.Any())
        {
            throw new NotFoundException("Session not found");
        }
        return mapper.Map<IEnumerable<MessageResponse>>(userSession.Messages);
    }
}
