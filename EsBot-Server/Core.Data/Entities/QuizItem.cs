using System.ComponentModel.DataAnnotations;
using Core.Data.DTOs.Requests;

namespace Core.Data.Entities;

public record QuizItem
{
    [Key]
    public Guid Id { get; set; }

    public Guid QuizRequestId { get; set; }

    [Required]
    public string QuestionText { get; set; } = string.Empty;

    // Stores expected answer structure (e.g., JSON or Keywords)
    public string ExpectedAnswerCriteria { get; set; } = string.Empty;

    // Navigation Properties
    public QuizRequest QuizRequest { get; set; } = null!;
}
