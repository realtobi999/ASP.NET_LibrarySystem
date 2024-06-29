using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Dtos.Reviews;

namespace LibrarySystem.Domain.Entities;

public class BookReview
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("book_id")]
    public Guid BookId { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Range(0,10), Column("rating")]
    public double Rating { get; set; }

    [Required, Column("text")]
    public string? Text { get; set; }

    [Required, Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    // relationships

    public Book? Book { get; set; }

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