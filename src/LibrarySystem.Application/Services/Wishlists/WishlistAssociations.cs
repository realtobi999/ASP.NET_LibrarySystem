using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Wishlists;

public class WishlistAssociations : IWishlistAssociations
{
    private readonly IRepositoryManager _repository;

    public WishlistAssociations(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public void CleanBooks(Wishlist wishlist)
    {
        foreach (var wishlistBook in wishlist.WishlistBooks)
        {
            _repository.Associations.RemoveWishlistBook(wishlistBook);
        }
    }

    public async Task AssignBooks(IEnumerable<Guid> booksIds, Wishlist wishlist)
    {
        var task = booksIds.Select(async bookId =>
        {
            var book = await _repository.Book.Get(bookId) ?? throw new NotFound404Exception(nameof(Book), bookId);
            _repository.Associations.CreateWishlistBook(new WishlistBook
            {
                Wishlist = wishlist,
                WishlistId = wishlist.Id,
                Book = book,
                BookId = book.Id
            });
        });

        await Task.WhenAll(task);
    }
}
