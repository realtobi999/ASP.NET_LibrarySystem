using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Services.Books;

namespace LibrarySystem.Application.Services.Books;

internal sealed class BookPopularityCalculator : IBookPopularityCalculator
{
    public const double BORROW_MULTIPLIER = 1.5;
    public const double REVIEW_MULTIPLIER = 1.5;
    public const double HIGH_RATING_MULTIPLIER = 1.3;
    public const double LOW_RATING_MULTIPLIER = 1.05;

    public double CalculatePopularityScore(Book book)
    {
        var popularityScore = 100.00;

        foreach (var _ in book.Borrows)
        {
            popularityScore *= BORROW_MULTIPLIER;
        }

        foreach (var review in book.BookReviews)
        {
            popularityScore *= REVIEW_MULTIPLIER;

            if (review.Rating >= 5.5)
            {
                popularityScore *= HIGH_RATING_MULTIPLIER;
            }
            else
            {
                popularityScore *= LOW_RATING_MULTIPLIER;
            }
        }

        return popularityScore;
    }
}

