using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBookService : IBaseService<Book>
{
    Task<Book> GetAsync(string isbn);
    Task UpdateAvailability(Book book, bool isAvailable);
    Task UpdatePopularity(Book book, double popularity);
}
