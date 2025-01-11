using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Authors;

public record class AuthorDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DateTimeOffset Birthday { get; init; }
    public required Picture? Picture { get; init; }
}
