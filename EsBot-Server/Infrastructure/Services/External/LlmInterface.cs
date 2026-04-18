using Core.Data.DTOs;
using Core.Data.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Infrastructure.Services.External;

public class LlmInterface(IQuizRepository quizRepository): ILlmInterface
{
    public async Task<string> Ask(string message)
    {
        return $" Really from a real LLm Answer {message}";
    }

    public async Task<Quiz> CreateQuiz(QuizRequest quiz)
    {

        return new Quiz
        {
            Question = quiz.Topic,
            Items =
            [
                new QuizItem { QuestionText = $"What is a class in {quiz.Topic}?" },
                new QuizItem { QuestionText = $"Explain inheritance in {quiz.Topic}." },
                new QuizItem { QuestionText = $"What is encapsulation in {quiz.Topic}?" }
            ]
        };
    }
}