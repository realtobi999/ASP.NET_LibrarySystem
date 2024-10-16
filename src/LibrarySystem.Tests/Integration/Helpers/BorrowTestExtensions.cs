using Bogus;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

public static class BorrowTestExtensions
{
    private static readonly Faker<Borrow> _borrowFaker = new Faker<Borrow>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.BorrowDate, _ => DateTimeOffset.Now)
        .RuleFor(b => b.DueDate, _ => DateTimeOffset.Now.AddMonths(1));

    public static Borrow WithFakeData(this Borrow borrow, Book book, User user)
    {
        var fakeBorrow = _borrowFaker.Generate();
        fakeBorrow.BookId = book.Id;
        fakeBorrow.UserId = user.Id;

        return fakeBorrow;
    }

    public static CreateBorrowDto ToCreateBorrowDto(this Borrow borrow)
    {
        return new CreateBorrowDto
        {
            Id = borrow.Id,
            BookId = borrow.BookId,
            UserId = borrow.UserId,
        };
    }
}
