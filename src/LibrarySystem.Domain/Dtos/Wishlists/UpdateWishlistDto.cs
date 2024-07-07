namespace LibrarySystem.Domain;

public record class UpdateWishlistDto
{
    public string? Name { get; set; }
    public List<Guid>? BookIds { get; set; }
}
