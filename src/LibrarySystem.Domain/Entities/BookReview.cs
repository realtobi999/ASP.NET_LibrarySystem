using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class BookReview : IDtoSerialization<BookReviewDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("book_id")]
    public Guid BookId { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Range(0, 10), Column("rating")]
    public double Rating { get; set; }

    [Required, Column("text")]
    public string? Text { get; set; }

    [Required, Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    // relationships

    [JsonIgnore]
    public Book? Book { get; set; }

    /// <inheritdoc/>
    public BookReviewDto ToDto()
    {
        return new BookReviewDto
        {
            Id = this.Id,
            BookId = this.BookId,
            UserId = this.UserId,
            Rating = this.Rating,
            Text = this.Text,
            CreatedAt = this.CreatedAt
        };
    }
}