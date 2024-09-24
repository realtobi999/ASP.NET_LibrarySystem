using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Reviews;

public record class CreateBookReviewDto
{
    public Guid? Id { get; set; }

    [Required]
    public Guid BookId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required, Range(0, 10)]
    public double Rating { get; set; }

    [Required, MaxLength(555)]
    public string? Text { get; set; }
}