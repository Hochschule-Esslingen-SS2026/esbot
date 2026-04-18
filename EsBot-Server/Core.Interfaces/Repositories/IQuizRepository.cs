using Core.Data.Entities;

namespace Core.Interfaces.Repositories;

public interface IQuizRepository
{
    Task<QuizRequest> AddQuizRequest(QuizRequest quizRequest);
}