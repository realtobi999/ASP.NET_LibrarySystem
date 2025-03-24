using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;

namespace LibrarySystem.Application.Core.Validators;

public class WishlistValidator : IValidator<Wishlist>
{
    private readonly IRepositoryManager _repository;

    public WishlistValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> ValidateAsync(Wishlist wishlist)
    {
        // validate that the user exists
        if (await _repository.User.GetAsync(wishlist.UserId) is null)
        {
            return (false, new NotFound404Exception(nameof(User), wishlist.UserId));
        }

        // validate that the assigned books exists
        foreach (var bookId in wishlist.Books.Select(b => b.Id))
        {
            var book = await _repository.Book.GetAsync(bookId);

            if (book is null)
            {
                return (false, new NotFound404Exception(nameof(Book), bookId));
            }
        }

        return (true, null);
    }
}