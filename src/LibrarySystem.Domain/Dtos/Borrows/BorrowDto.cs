namespace LibrarySystem.Domain.Dtos.Borrows;

public class BorrowDto
{
    public Guid Id { get; init; }
    public Guid BookId { get; init; }
    public Guid UserId { get; init; }
    public DateTimeOffset BorrowDate { get; init; }
    public DateTimeOffset DueDate { get; init; }
    public bool IsReturned { get; init; }
}
