using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Reviews;

public record class CreateBookReviewDto
{
    public Guid? Id { get; init; }

    [Required]
    public Guid BookId { get; init; }

    [Required]
    public Guid UserId { get; init; }

    [Required, Range(0, 10)]
    public double Rating { get; init; }

    [Required, MaxLength(555)]
    public string? Text { get; init; }
}