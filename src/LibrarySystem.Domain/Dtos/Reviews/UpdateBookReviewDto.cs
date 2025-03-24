using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Reviews;

public record UpdateBookReviewDto
{
    [Required, MaxLength(555)]
    public required string Text { get; init; }

    [Required, Range(0, 10)]
    public required double Rating { get; init; }
}