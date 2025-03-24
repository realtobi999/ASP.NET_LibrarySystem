using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Borrow : IDtoSerialization<BorrowDto>
{
    // core properties

    [Required, Column("id")]
    public required Guid Id { get; init; }

    [Required, Column("book_id")]
    public required Guid BookId { get; set; }

    [Required, Column("user_id")]
    public required Guid UserId { get; set; }

    [Required, Column("borrow_date")]
    public required DateTimeOffset BorrowDate { get; set; }

    [Required, Column("borrow_due")]
    public required DateTimeOffset DueDate { get; set; }

    [Required, Column]
    public required bool IsReturned { get; set; }

    // relationships

    [JsonIgnore]
    public virtual User? User { get; set; }

    [JsonIgnore]
    public virtual Book? Book { get; set; }

    /// <inheritdoc/>
    public BorrowDto ToDto()
    {
        return new BorrowDto
        {
            Id = this.Id,
            BookId = this.BookId,
            UserId = this.UserId,
            BorrowDate = this.BorrowDate,
            DueDate = this.DueDate,
            IsReturned = this.IsReturned
        };
    }

    public void UpdateIsReturned(bool isReturned)
    {
        IsReturned = isReturned;
    }
}