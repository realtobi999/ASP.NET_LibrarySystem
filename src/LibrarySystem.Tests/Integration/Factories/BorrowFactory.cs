using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class BorrowFactory
{
    private static readonly Faker<Borrow> _factory = new Faker<Borrow>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.BorrowDate, _ => DateTimeOffset.Now)
        .RuleFor(b => b.DueDate, _ => DateTimeOffset.Now.AddMonths(1));

    public static Borrow CreateWithFakeData(Book book, User user)
    {
        var borrow = _factory.Generate();
        borrow.BookId = book.Id;
        borrow.UserId = user.Id;

        return borrow;
    }
}
