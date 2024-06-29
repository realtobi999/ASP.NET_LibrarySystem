namespace LibrarySystem.Domain.Dtos.Reviews;

public record class BookReviewDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    public double Rating { get; set; }
    public string? Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}