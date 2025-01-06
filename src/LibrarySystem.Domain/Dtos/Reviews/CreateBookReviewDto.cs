using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Reviews;

public record class CreateBookReviewDto
{
    public Guid? Id { get; init; }

    [Required]
    public required Guid BookId { get; init; }

    [Required]
    public required Guid UserId { get; init; }

    [Required, Range(0, 10)]
    public required double Rating { get; init; }

    [Required, MaxLength(555)]
    public required string Text { get; init; }
}