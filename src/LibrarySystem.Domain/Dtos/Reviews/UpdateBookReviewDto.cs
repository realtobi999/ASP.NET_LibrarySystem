using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Reviews;

public record class UpdateBookReviewDto
{   
    [Required]
    public string? Text { get; set; }
}
