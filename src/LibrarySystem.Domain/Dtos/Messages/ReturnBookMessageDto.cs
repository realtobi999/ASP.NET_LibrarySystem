namespace LibrarySystem.Domain;

public record class ReturnBookMessageDto
{
    public required string Username { get; set; }
    public required string BookTitle { get; set; }
    public required string BookISBN { get; set; }
}
