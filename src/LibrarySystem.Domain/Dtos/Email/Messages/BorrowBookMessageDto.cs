namespace LibrarySystem.Domain.Dtos.Email.Messages;

public record class BorrowBookMessageDto
{
    public required string UserEmail { get; init; }
    public required string Username { get; init; }
    public required string BookTitle { get; init; }
    public required string BookISBN { get; init; }
    public required string BorrowDueDate { get; init; }
}
