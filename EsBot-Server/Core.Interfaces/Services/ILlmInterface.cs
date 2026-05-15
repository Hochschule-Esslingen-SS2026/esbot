using Core.Data.DTOs;
using Core.Data.Entities;

namespace Core.Interfaces.Services;

public interface ILlmInterface
{
    public Task<string> Ask(string message);

    public Task<Quiz> CreateQuiz(QuizRequest mesage);
    public Task<string> Evaluate(string questionText,string userAnswer);

}
