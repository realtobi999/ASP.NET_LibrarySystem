using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class BookReviewTestExtensions
{
    public static CreateBookReviewDto ToCreateBookReviewDto(this BookReview bookReview)
    {
        return new CreateBookReviewDto
        {
            Id = bookReview.Id,
            BookId = bookReview.BookId,
            UserId = bookReview.UserId,
            Rating = bookReview.Rating,
            Text = bookReview.Text
        };
    }
}