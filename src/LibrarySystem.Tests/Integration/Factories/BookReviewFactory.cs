using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class BookReviewFactory
{
    private static readonly Faker<BookReview> _factory = new Faker<BookReview>()
        .RuleFor(br => br.Id, f => f.Random.Guid())
        .RuleFor(br => br.Rating, f => f.Random.Double(0, 10))
        .RuleFor(br => br.Text, f => f.Lorem.Paragraph())
        .RuleFor(br => br.CreatedAt, f => f.Date.PastOffset());

    public static BookReview CreateWithFakeData(Book book, User user)
    {
        var review = _factory.Generate();

        review.BookId = book.Id;
        review.UserId = user.Id;

        return review;
    }
}
