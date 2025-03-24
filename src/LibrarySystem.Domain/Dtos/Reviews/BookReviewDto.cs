namespace LibrarySystem.Domain.Dtos.Reviews;

public record BookReviewDto
{
    public required Guid Id { get; init; }
    public required Guid BookId { get; init; }
    public required Guid UserId { get; init; }
    public required double Rating { get; init; }
    public required string Text { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}