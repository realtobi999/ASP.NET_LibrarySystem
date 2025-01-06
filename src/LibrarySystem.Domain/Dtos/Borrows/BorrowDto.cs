namespace LibrarySystem.Domain.Dtos.Borrows;

public class BorrowDto
{
    public required Guid Id { get; init; }
    public required Guid BookId { get; init; }
    public required Guid UserId { get; init; }
    public required DateTimeOffset BorrowDate { get; init; }
    public required DateTimeOffset DueDate { get; init; }
    public required bool IsReturned { get; init; }
}
