namespace LibrarySystem.Domain;

public record class StaffDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}