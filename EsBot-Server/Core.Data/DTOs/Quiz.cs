using Core.Data.Entities;

namespace Core.Data.DTOs;

public record Quiz
{
    public required QuizItem[] Items { get; set; }
    public required string Topic { get; set; }
}
