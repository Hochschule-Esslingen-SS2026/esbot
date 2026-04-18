namespace Core.Data.DTOs.Responses;

public class QuizItemResponse
{
    public required int Id { get; init; }

    public required string QuestionText { get; set; }
}