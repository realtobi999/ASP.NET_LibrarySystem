using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class User : IDtoSerialization<UserDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("username")]
    public string? Username { get; set; }

    [Required, Column("email")]
    public string? Email { get; set; }

    [Required, Column("password")]
    public string? Password { get; set; }

    // relationships

    [JsonIgnore]
    public Picture? ProfilePicture { get; set; }

    [JsonIgnore]
    public ICollection<BookReview> BookReviews { get; set; } = [];

    [JsonIgnore]
    public ICollection<Wishlist> Wishlists { get; set; } = [];

    [JsonIgnore]
    public ICollection<Borrow> Borrows { get; set; } = [];

    /// <inheritdoc/>
    public UserDto ToDto()
    {
        return new UserDto
        {
            Id = this.Id,
            Username = this.Username,
            Email = this.Email,
            ProfilePicture = this.ProfilePicture,
            Reviews = this.BookReviews.Select(br => br.ToDto()).ToList(),
            Wishlists = this.Wishlists.Select(w => w.ToDto()).ToList(),
            Borrows = this.Borrows.Select(b => b.ToDto()).ToList()
        };
    }

    public void Update(UpdateUserDto dto)
    {
        Email = dto.Email;
        Username = dto.Username;
    }
}