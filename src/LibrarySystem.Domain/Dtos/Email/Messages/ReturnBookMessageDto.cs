namespace LibrarySystem.Domain.Dtos.Email.Messages;

public record ReturnBookMessageDto
{
    public required string UserEmail { get; init; }
    public required string Username { get; init; }
    public required string BookTitle { get; init; }
    public required string BookIsbn { get; init; }
}