namespace Core.Data.Entities;

public record Person
{
    public Guid Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}