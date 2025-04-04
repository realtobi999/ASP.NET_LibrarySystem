using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal static class BookReviewFactory
{
    private static readonly Faker<BookReview> Factory = new Faker<BookReview>()
        .RuleFor(br => br.Id, f => f.Random.Guid())
        .RuleFor(br => br.Rating, f => f.Random.Double(0, 10))
        .RuleFor(br => br.Text, f => f.Lorem.Paragraph())
        .RuleFor(br => br.CreatedAt, _ => DateTimeOffset.UtcNow);

    public static BookReview CreateWithFakeData(Book book, User user)
    {
        var review = Factory.Generate();

        review.BookId = book.Id;
        review.UserId = user.Id;

        return review;
    }
}