namespace Core.Data.DTOs.Requests;

public record CreateQuizRequest
{
    public required Guid UserSessionId { get; set; }

    public required string Topic { get; set; }
}