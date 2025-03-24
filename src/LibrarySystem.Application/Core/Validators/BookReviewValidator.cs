using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;

namespace LibrarySystem.Application.Core.Validators;

public class BookReviewValidator : IValidator<BookReview>
{
    private readonly IRepositoryManager _repository;

    public BookReviewValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> ValidateAsync(BookReview review)
    {
        // validate that the book exists
        if (await _repository.Book.GetAsync(review.BookId) is null)
        {
            return (false, new NotFound404Exception(nameof(Book), review.BookId));
        }

        // validate that the user exists
        if (await _repository.User.GetAsync(review.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), review.UserId));
        }

        return (true, null);
    }
}