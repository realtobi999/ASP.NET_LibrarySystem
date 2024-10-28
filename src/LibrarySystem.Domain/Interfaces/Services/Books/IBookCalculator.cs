using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services.Books;

public interface IBookCalculator
{
    double CalculatePopularityScore(Book book);
}
