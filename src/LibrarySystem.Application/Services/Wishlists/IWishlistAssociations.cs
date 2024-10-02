using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Services.Wishlists;

public interface IWishlistAssociations
{
    public void AssignBooks(IEnumerable<Guid> booksIds, Wishlist wishlist);
    public void CleanBooks(Wishlist wishlist);
}
