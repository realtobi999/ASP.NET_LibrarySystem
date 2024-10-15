using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Core.Validators;

public class BorrowValidator : IValidator<Borrow>
{
    private readonly IRepositoryManager _repository;

    public BorrowValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> ValidateAsync(Borrow borrow)
    {
        // validate that the book exists
        var book = await _repository.Book.GetAsync(borrow.BookId);
        if (book is null)
        {
            return (false, new NotFound404Exception(nameof(Book), borrow.BookId));
        }

        // validate that the user exists
        if (await _repository.User.GetAsync(borrow.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), borrow.UserId));
        }

        // validate that the borrow's BorrowDate is before the DueDate
        if (borrow.BorrowDate >= borrow.DueDate)
        {
            return (false, new BadRequest400Exception("Borrow date must be before the due date."));
        }

        // validate that borrow cannot be set to returned after the DueDate
        if (!borrow.IsReturned && DateTimeOffset.UtcNow >= borrow.DueDate)
        {
            return (false, new BadRequest400Exception("The book cannot be set to returned after the due date."));
        }

        // validate that the book is set to unavailable if the borrow is not returned
        if (!borrow.IsReturned && book.IsAvailable)
        {
            return (false, new Conflict409Exception("The book should be marked as unavailable if not returned."));
        }


        return (true, null);
    }
}
