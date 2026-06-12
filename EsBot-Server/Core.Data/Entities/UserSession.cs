using System.ComponentModel.DataAnnotations;
using Core.Data.DTOs.Requests;

namespace Core.Data.Entities;

public record UserSession
{
    public UserSession(string externalUserId)
    {
        Id = Guid.NewGuid();
        ExternalUserId = externalUserId;
    }


    [Key] public Guid Id { get; set; }

    [Required][MaxLength(100)] public string ExternalUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset LastInteractionAt { get; set; } = DateTime.UtcNow;
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<QuizRequest> QuizRequests { get; set; } = new List<QuizRequest>();
}
