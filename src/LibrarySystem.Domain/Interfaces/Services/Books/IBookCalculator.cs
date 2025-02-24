using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services.Books;

public interface IBookPopularityCalculator
{
    double CalculatePopularityScore(Book book);
}
