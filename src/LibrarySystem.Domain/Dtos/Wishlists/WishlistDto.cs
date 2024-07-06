using LibrarySystem.Domain.Dtos.Books;

namespace LibrarySystem.Domain.Dtos.Wishlists;

public record class WishlistDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Name { get; set; }
    public IEnumerable<BookDto> Books { get; set; } = [];
}