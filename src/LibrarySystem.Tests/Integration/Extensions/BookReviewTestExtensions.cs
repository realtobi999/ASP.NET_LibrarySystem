using Bogus;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Extensions;

public static class BookReviewTestExtensions
{
    private static readonly Faker<BookReview> _bookReviewFaker = new Faker<BookReview>()
        .RuleFor(br => br.Id, f => f.Random.Guid())
        .RuleFor(br => br.Rating, f => f.Random.Double(0, 10))
        .RuleFor(br => br.Text, f => f.Lorem.Paragraph())
        .RuleFor(br => br.CreatedAt, f => f.Date.PastOffset());

    public static BookReview WithFakeData(this BookReview bookReview, Book book, User user)
    {
        var review = _bookReviewFaker.Generate();

        review.BookId = book.Id;
        review.UserId = user.Id;

        return review;
    }

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