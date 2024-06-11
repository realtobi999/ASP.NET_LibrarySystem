using Bogus;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Extensions;

public static class BorrowTestExtensions
{
    private static readonly Faker<Borrow> _borrowFaker = new Faker<Borrow>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.BorrowDate, DateTimeOffset.UtcNow);

    public static Borrow WithFakeData(this Borrow borrow, Guid bookId, Guid userId)
    {
        var fakeBorrow = _borrowFaker.Generate();
        fakeBorrow.BookId = bookId;
        fakeBorrow.UserId = userId;

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
