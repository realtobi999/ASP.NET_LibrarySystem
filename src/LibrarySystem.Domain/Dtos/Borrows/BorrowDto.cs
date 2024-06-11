using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Borrows;

public class BorrowDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset BorrowDate { get; set; }
    public DateTimeOffset BorrowDue { get; set; }
}
