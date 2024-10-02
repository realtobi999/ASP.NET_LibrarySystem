using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Authors;

public record class AuthorDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset Birthday { get; init; }
    public Picture? Picture { get; init; }
}
