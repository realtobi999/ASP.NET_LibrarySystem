using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBookService : IBaseService<Book>
{
    Task<IEnumerable<Book>> IndexRecommendedAsync(User user);
    Task<Book> GetAsync(string isbn);
    Task UpdateAvailabilityAsync(Book book, bool isAvailable);
    Task UpdatePopularityAsync(Book book, double popularity);
}
