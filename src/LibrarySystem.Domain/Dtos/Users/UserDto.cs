using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Dtos.Users;

public record class UserDto
{
    public Guid Id { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
    public Picture? ProfilePicture { get; init; }
    public List<WishlistDto> Wishlists { get; set; } = [];
    public List<BookReviewDto> Reviews { get; set; } = [];
    public List<BorrowDto> Borrows { get; set; } = [];
}
