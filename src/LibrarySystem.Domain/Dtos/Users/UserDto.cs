using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UserDto
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required Picture? ProfilePicture { get; init; }
    public required List<WishlistDto> Wishlists { get; set; } = [];
    public required List<BookReviewDto> Reviews { get; set; } = [];
    public required List<BorrowDto> Borrows { get; set; } = [];
}
