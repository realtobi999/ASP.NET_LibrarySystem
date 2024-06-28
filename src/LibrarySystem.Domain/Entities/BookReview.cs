using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string? CreatedAt { get; set; }

    // relationships

    public Book? Book { get; set; }
}