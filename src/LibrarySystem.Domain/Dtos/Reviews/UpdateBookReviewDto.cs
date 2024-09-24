using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Reviews;

public record class UpdateBookReviewDto
{
    [Required, MaxLength(555)]
    public string? Text { get; set; }

    [Required, Range(0, 10)]
    public double Rating { get; set; }
}
