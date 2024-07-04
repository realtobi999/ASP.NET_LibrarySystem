using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Entities.Relationships;

namespace LibrarySystem.Domain.Entities;

public class Wishlist
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
}
