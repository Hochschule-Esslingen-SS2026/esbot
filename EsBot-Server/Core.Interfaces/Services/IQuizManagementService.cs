using Core.Data.DTOs.Requests;
using Core.Data.DTOs.Responses;

namespace Core.Interfaces.Services;

public interface IQuizManagementService
{
    Task<QuizRequestResponse> RequestQuiz(CreateQuizRequest request);
}