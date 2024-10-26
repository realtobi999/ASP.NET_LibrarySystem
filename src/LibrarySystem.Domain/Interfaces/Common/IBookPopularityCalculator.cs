using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Common;

public interface IBookPopularityCalculator
{
    double CalculatePopularityScore(Book book);
}
