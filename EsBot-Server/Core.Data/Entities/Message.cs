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
        CreatedAt = DateTime.UtcNow;
    }
    [Key] public Guid Id { get; init; }

    [Required]
    public Guid UserSessionId { get; set; }

    [Required]
    public bool Role { get; set; } // If True User if False AI

    [Required]
    public string Content { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public UserSession UserSession { get; set; } = null!;
};
