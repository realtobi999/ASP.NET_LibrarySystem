using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services.Books;

public interface IBookRecommender
{
    Task<IEnumerable<Book>> IndexRecommendedAsync(User user);
}
