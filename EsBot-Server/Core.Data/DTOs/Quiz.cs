using Core.Data.Entities;

namespace Core.Data.DTOs;

public record Quiz
{
    public QuizItem[] Items { get; set; }
    public string Question { get; set; }
}