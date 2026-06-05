namespace Core.Data.DTOs.Responses;

public class QuizItemResponse
{
    public required Guid Id { get; init; }

    public required string QuestionText { get; set; }
}
