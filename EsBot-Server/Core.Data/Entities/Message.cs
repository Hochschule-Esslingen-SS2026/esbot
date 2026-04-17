using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities;

public record Message
{

    public Message(Guid userSessionId, bool role, string content)
    {
        Id = Guid.NewGuid();
        UserSessionId = userSessionId;
        Role = role;
        Content = content;
        Timestamp = DateTime.UtcNow;
    }

    public Message()
    {
        
    }

    [Key] public required Guid Id { get; init; }
    
    [Required]
    public required Guid UserSessionId { get; set; }

    [Required]
    public required bool Role { get; set; } // If True User if False AI

    [Required]
    public required string Content { get; set; }

    [Required] public required DateTime Timestamp { get; init; }

    // Navigation Property
    public UserSession UserSession { get; set; } = null!;
};