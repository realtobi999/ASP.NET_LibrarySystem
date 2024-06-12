using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Domain.Dtos.Borrows;

public record class CreateBorrowDto
{
    public Guid? Id { get; set; }

    [Required]
    public Guid BookId { get; set; }

    [Required]
    public Guid UserId { get; set; }
}
