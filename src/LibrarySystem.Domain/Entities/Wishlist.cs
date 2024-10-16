using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Wishlist : IDtoSerialization<WishlistDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Column("name")]
    public string? Name { get; set; }

    // relationships

    public User? User { get; set; }
    public ICollection<WishlistBook> WishlistBooks { get; set; } = [];

    /// <inheritdoc/>
    public WishlistDto ToDto()
    {
        var books = this.WishlistBooks.Where(wb => wb.Book is not null)
                                      .Select(wb => wb.Book!.ToDto())
                                      .ToList();
        return new WishlistDto
        {
            Id = this.Id,
            UserId = this.UserId,
            Name = this.Name,
            Books = books,
        };
    }

    public void Update(UpdateWishlistDto dto)
    {
        Name = dto.Name;

        // clean previous attached books and assign new
        WishlistBooks.Clear();
        
        foreach (var bookId in dto.BookIds)
        {
            WishlistBooks.Add(new WishlistBook
            {
                WishlistId = Id,
                BookId = bookId,
            });
        }
    }
}
