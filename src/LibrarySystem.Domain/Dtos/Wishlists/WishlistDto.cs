using LibrarySystem.Domain.Dtos.Books;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class WishlistDto
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public required IEnumerable<BookDto> Books { get; init; } = [];
}