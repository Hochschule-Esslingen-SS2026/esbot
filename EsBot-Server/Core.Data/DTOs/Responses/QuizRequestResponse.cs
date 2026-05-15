namespace Core.Data.DTOs.Responses;

public class QuizRequestResponse
{
    public required Guid Id { get; init; }
    public required string Topic { get; init; }
    public required ICollection<QuizItemResponse> QuizItems { get; set; }
}
