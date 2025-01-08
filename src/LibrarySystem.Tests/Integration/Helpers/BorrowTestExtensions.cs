using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class BorrowTestExtensions
{
    public static CreateBorrowDto ToCreateBorrowDto(this Borrow borrow)
    {
        return new CreateBorrowDto
        {
            Id = borrow.Id,
            BookId = borrow.BookId,
            UserId = borrow.UserId,
        };
    }
}
