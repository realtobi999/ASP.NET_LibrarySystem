using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBorrowService : IBaseService<Borrow>
{
    Task<Borrow> GetAsync(Guid bookId, Guid userId);
    Task ReturnAsync(Borrow borrow, string jwt, Func<Book, Task> updateBookAsync);
    Task ReturnAsync(Borrow borrow, Book book, string jwt, Func<Book, Task> updateBookAsync);
}
