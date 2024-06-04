namespace LibrarySystem.Domain.Dtos.Books;

public record class BookDto
{
    public Guid Id { get; set; }
    public string? ISBN { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int PagesCount { get; set; } 
    public DateTimeOffset PublishedAt { get; set; }
    public byte[]? CoverPicture { get; set; }
}
