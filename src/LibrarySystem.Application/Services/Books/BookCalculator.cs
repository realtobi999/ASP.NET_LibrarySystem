using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Services.Books;

namespace LibrarySystem.Application.Services.Books;

internal sealed class BookCalculator : IBookCalculator
{
    public const double BORROW_POPULARITY_VALUE = 50;
    public const double REVIEW_POPULARITY_VALUE = 25;
    public const double POPULARITY_PENALTY_MULTIPLIER = 0.20;

    public double CalculatePopularityScore(Book book)
    {
        var popularity = book.Popularity;

        // get recent borrows and foreach borrow add a set value to the popularity
        var recentBorrowsCount = book.Borrows.Count(b => b.BorrowDate.Date == DateTime.Now.Date);

        if (recentBorrowsCount > 0)
        {
            popularity += recentBorrowsCount * BORROW_POPULARITY_VALUE;
        }

        // get recent reviews with good rating and foreach review add a set value to the popularity
        var recentPositiveReviewsCount = book.BookReviews.Count(br => br.CreatedAt.Date == DateTime.Now.Date && br.Rating > BookReview.RATING_MIDDLE_VALUE);

        if (recentPositiveReviewsCount > 0)
        {
            popularity += recentPositiveReviewsCount * REVIEW_POPULARITY_VALUE;
        }

        // if no new reviews or borrows multiply the popularity by a penalty
        if (recentBorrowsCount == 0 || recentPositiveReviewsCount == 0)
        {
            popularity *= POPULARITY_PENALTY_MULTIPLIER;
        }

        return popularity;
    }
}

