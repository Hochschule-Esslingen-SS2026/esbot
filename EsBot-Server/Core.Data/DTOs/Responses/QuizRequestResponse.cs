namespace Core.Data.DTOs.Responses;

public class QuizRequestResponse
{
    public required int Id { get; init; }

    public required string Topic { get; set; }

    public required ICollection<QuizItemResponse> QuizItems { get; set; }
}