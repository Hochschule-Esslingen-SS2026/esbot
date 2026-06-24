using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;

namespace Core.Interfaces.Services;

public interface IChatService
{
    public Task<UserSession> CreateNewSession(string externalUserId);

    public Task<MessageResponse> AskQuestion(QuestionRequest question);
    public Task<QuizRequestResponse> RequestQuiz(CreateQuizRequest request);
    public Task<IEnumerable<MessageResponse>> GetSession(Guid sessionId);
    public Task<EvaluationResultResponse> EvaluateAnswer(AnswerEvaluationRequest request);
}
