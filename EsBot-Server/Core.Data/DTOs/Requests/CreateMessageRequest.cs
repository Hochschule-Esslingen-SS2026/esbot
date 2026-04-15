using System.ComponentModel.DataAnnotations;

namespace Core.Data.DTOs.Requests;

public record CreateMessageRequest
{
    [Required]
    public string Content { get; init; }
}