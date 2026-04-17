namespace Core.Data.DTOs.Requests;

public record QuestionRequest
{
    public required Guid UserSessionId { get; set; }
    
    public required string Question { get; set; }
}