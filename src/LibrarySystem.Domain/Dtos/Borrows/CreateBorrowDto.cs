using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Borrows;

public record class CreateBorrowDto
{
    public Guid? Id { get; init; }

    [Required]
    public Guid BookId { get; init; }

    [Required]
    public Guid UserId { get; init; }
}
