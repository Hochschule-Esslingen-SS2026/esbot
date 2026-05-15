using Core.Data.DTOs;
using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Infrastructure.Services.External;

public class LlmInterface: ILlmInterface
{
    public async Task<string> Ask(string message)
    {
        return $" Really from a real LLm Answer {message}";
    }

    public async Task<Quiz> CreateQuiz(QuizRequest quiz)
    {
        return new Quiz
        {
            Topic = quiz.Topic,
            Items = new[]
            {
                new Core.Data.Entities.QuizItem { QuestionText = $"What is a class in {quiz.Topic}?" },
                new Core.Data.Entities.QuizItem { QuestionText = $"Explain inheritance in {quiz.Topic}." },
                new Core.Data.Entities.QuizItem { QuestionText = $"What is encapsulation in {quiz.Topic}?" }
            }
        };
    }

    public async Task<string> Evaluate(string questionText, string userAnswer)
    {
        return "Not Mocked normal answer Correct";
    }
}
