using LibrarySystem.Domain.Dtos.Books;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class WishlistDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string? Name { get; init; }
    public IEnumerable<BookDto> Books { get; init; } = [];
}