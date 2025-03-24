namespace LibrarySystem.Domain.Dtos.Email.Messages;

public record BorrowBookMessageDto
{
    public required string UserEmail { get; init; }
    public required string Username { get; init; }
    public required string BookTitle { get; init; }
    public required string BookIsbn { get; init; }
    public required string BorrowDueDate { get; init; }
}