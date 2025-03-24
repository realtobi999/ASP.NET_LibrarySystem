using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal static class BorrowFactory
{
    private static readonly Faker<Borrow> Factory = new Faker<Borrow>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.BorrowDate, _ => DateTimeOffset.Now) // so we can avoid validation problems in the API
        .RuleFor(b => b.DueDate, _ => DateTimeOffset.Now.AddMonths(1))
        .RuleFor(b => b.IsReturned, _ => false);

    public static Borrow CreateWithFakeData(Book book, User user)
    {
        var borrow = Factory.Generate();

        borrow.BookId = book.Id;
        borrow.UserId = user.Id;

        return borrow;
    }
}