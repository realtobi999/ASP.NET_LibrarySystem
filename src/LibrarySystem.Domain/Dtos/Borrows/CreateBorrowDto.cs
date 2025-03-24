using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Borrows;

public record CreateBorrowDto
{
    public Guid? Id { get; init; }

    [Required]
    public required Guid BookId { get; init; }

    [Required]
    public required Guid UserId { get; init; }
}