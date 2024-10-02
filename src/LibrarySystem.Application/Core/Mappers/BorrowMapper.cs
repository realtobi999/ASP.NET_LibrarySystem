using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class BorrowMapper : IBorrowMapper
{
    public Borrow CreateFromDto(CreateBorrowDto dto)
    {
        return new Borrow
        {
            Id = dto.Id ?? Guid.NewGuid(),
            BookId = dto.BookId,
            UserId = dto.UserId,
            BorrowDate = DateTimeOffset.UtcNow,
            DueDate = DateTimeOffset.UtcNow.AddMonths(1),
            IsReturned = false,
        };
    }
}
