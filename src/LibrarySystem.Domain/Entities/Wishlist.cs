using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Dtos.Wishlists;
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

    [Required, Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    // relationships

    public User? User { get; set; }
    public ICollection<Book> Books { get; set; } = [];

    /// <inheritdoc/>
    public WishlistDto ToDto()
    {
        var books = this.Books.Select(wb => wb.ToDto()).ToList();
        return new WishlistDto
        {
            Id = this.Id,
            UserId = this.UserId,
            Name = this.Name,
            Books = books,
        };
    }

    public void Update(UpdateWishlistDto dto, IEnumerable<Book> books)
    {
        Name = dto.Name;

        // clean previous attached books and assign new
        this.Books.Clear();

        foreach (var book in books)
        {
            this.Books.Add(book);
        }
    }
}
