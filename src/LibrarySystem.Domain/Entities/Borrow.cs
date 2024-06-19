using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Dtos.Borrows;

namespace LibrarySystem.Domain.Entities;

public class Borrow
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("book_id")]
    public Guid BookId { get; set; }

    [Required, Column("user_id")]
    public Guid UserId { get; set; }

    [Required, Column("borrow_date")]
    public DateTimeOffset BorrowDate { get; set; }

    [Required, Column("borrow_due")]
    public DateTimeOffset DueDate { get; set; }

    [Required, Column]
    public bool IsReturned { get; set; }
    
    public BorrowDto ToDto()
    {
        return new BorrowDto
        {
            Id = this.Id,
            BookId = this.BookId,
            UserId = this.UserId,
            BorrowDate = this.BorrowDate,
            DueDate = this.DueDate,
            IsReturned = this.IsReturned,
        };
    }
}