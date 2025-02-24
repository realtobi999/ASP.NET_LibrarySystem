using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Services.Books;

namespace LibrarySystem.Application.Services.Books;

internal sealed class BookCalculator : IBookCalculator
{
    public const double BORROW_SCORE_INCREMENT = 50;
    public const double REVIEW_SCORE_INCREMENT = 25;
    public const double INACTIVITY_PENALTY_MULTIPLIER = 0.10;

    public const double RECENT_ACTIVITY_DAYS = 12; // days

    public double CalculatePopularityScore(Book book)
    {
        var popularity = book.Popularity;

        // get recent borrows and add a set value to the popularity for each borrow
        var recentBorrowCount = book.Borrows.Count(b => b.BorrowDate.Date >= DateTime.Now.Date.AddDays(-RECENT_ACTIVITY_DAYS));
        if (recentBorrowCount > 0)
        {
            popularity += recentBorrowCount * BORROW_SCORE_INCREMENT;
        }

        // get recent positive reviews and add a set value to the popularity for each review
        var recentPositiveReviewCount = book.BookReviews.Count(br => br.CreatedAt.Date > DateTime.Now.Date.AddDays(-RECENT_ACTIVITY_DAYS) && br.Rating > BookReview.RATING_MIDDLE_VALUE);
        if (recentPositiveReviewCount > 0)
        {
            popularity += recentPositiveReviewCount * REVIEW_SCORE_INCREMENT;
        }

        // if no new reviews or borrows, apply a penalty multiplier
        if (recentBorrowCount == 0 && recentPositiveReviewCount == 0)
        {
            popularity -= popularity * INACTIVITY_PENALTY_MULTIPLIER;
        }

        return popularity;
    }
}