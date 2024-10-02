using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBookService : IBaseService<Book>
{
    Task<Book> GetAsync(string isbn);
    Task SetAvailability(Book book, bool isAvailable);
}
