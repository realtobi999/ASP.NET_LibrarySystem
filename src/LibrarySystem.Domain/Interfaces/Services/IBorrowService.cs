using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBorrowService : IBaseService<Borrow>
{
    Task<Borrow> GetAsync(Guid bookId, Guid userId);
    Task ReturnAsync(Borrow borrow, string jwt);
    Task ReturnAsync(Borrow borrow, Book book, string jwt);
}
