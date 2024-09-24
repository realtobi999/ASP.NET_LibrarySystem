namespace LibrarySystem.Domain.Dtos.Messages;

public record class ReturnBookMessageDto
{
    public required string UserEmail { get; set; }
    public required string Username { get; set; }
    public required string BookTitle { get; set; }
    public required string BookISBN { get; set; }
}
