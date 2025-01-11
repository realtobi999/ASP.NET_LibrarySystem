﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class User : IDtoSerialization<UserDto>
{
    // core properties
    
    [Required, Column("id")]
    public required Guid Id { get; set; }

    [Required, Column("username")]
    public required string Username { get; set; }

    [Required, Column("email")]
    public required string Email { get; set; }

    [Required, Column("password")]
    public required string Password { get; set; }

    [Required, Column("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    // relationships

    [JsonIgnore]
    public virtual Picture? ProfilePicture { get; set; }

    [JsonIgnore]
    public virtual ICollection<BookReview> BookReviews { get; set; } = [];

    [JsonIgnore]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = [];

    [JsonIgnore]
    public virtual ICollection<Borrow> Borrows { get; set; } = [];

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