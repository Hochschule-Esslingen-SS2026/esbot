using AutoMapper;
using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;
using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Core.Services;

public class QuizManagementService(
    IMapper mapper,
    IQuizRepository quizRepository,
    ILlmInterface llmInterface)
    : IQuizManagementService
{
    public async Task<QuizRequestResponse> RequestQuiz(CreateQuizRequest topic)
    {
        var quizRequest = mapper.Map<QuizRequest>(topic);
        var quiz = await llmInterface.CreateQuiz(quizRequest);
        quizRequest.QuizItems = quiz.Items.ToList();

        await quizRepository.AddQuizRequest(quizRequest);

        return mapper.Map<QuizRequestResponse>(quizRequest);
    }
}