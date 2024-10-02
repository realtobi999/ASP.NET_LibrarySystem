namespace LibrarySystem.Domain.Dtos.Reviews;

public record class BookReviewDto
{
    public Guid Id { get; init; }
    public Guid BookId { get; init; }
    public Guid UserId { get; init; }
    public double Rating { get; init; }
    public string? Text { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}