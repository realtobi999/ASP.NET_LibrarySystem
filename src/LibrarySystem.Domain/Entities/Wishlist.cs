using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Wishlist : IDtoSerialization<WishlistDto>
{
    // core properties

    [Required, Column("id")]
    public required Guid Id { get; init; }

    [Required, Column("user_id")]
    public required Guid UserId { get; set; }

    [Required, Column("name")]
    public required string Name { get; set; }

    [Required, Column("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }

    // relationships

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = [];

    /// <inheritdoc/>
    public WishlistDto ToDto()
    {
        var books = this.Books.Select(wb => wb.ToDto()).ToList();
        return new WishlistDto
        {
            Id = this.Id,
            UserId = this.UserId,
            Name = this.Name,
            Books = books
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