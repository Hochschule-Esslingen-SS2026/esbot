namespace Core.Data.DTOs.Responses;

public record SessionResponse(Guid SessionId, string UserId, DateTimeOffset CreatedAt);
