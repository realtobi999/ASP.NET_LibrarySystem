using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class BookReview : IDtoSerialization<BookReviewDto>
{
    // core properties

    [Required, Column("id")]
    public required Guid Id { get; set; }

    [Required, Column("book_id")]
    public required Guid BookId { get; set; }

    [Required, Column("user_id")]
    public required Guid UserId { get; set; }

    [Required, Range(0, 10), Column("rating")]
    public required double Rating { get; set; }

    [Required, Column("text")]
    public required string Text { get; set; }

    [Required, Column("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    // constants

    public const double RATING_MIDDLE_VALUE = 5;

    // relationships


    [JsonIgnore]
    public virtual Book? Book { get; set; }
    [JsonIgnore]
    public virtual User? User { get; set; }

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

    public void Update(UpdateBookReviewDto dto)
    {
        Text = dto.Text;
        Rating = dto.Rating;
    }
}