using System.Security.Cryptography.X509Certificates;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Services.Wishlists;

public interface IWishlistAssociations
{
    public Task AssignBooksAsync(IEnumerable<Guid> booksIds, Wishlist wishlist);
}
