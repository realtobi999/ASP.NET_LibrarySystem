namespace LibrarySystem.Domain.Dtos.Messages;

public record class BorrowBookMessageDto
{
    public required string UserEmail { get; set; }
    public required string Username { get; set; }
    public required string BookTitle { get; set; }
    public required string BookISBN { get; set; }
    public required string BorrowDueDate { get; set; }
}
