using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Services.Wishlists;

public interface IWishlistAssociations
{
    public Task AssignBooks(IEnumerable<Guid> booksIds, Wishlist wishlist);
    public void CleanBooks(Wishlist wishlist);
}
