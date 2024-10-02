using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Wishlists;

public class WishlistService : IWishlistService
{
    private readonly IRepositoryManager _repository;
    private readonly IWishlistAssociations _associations;

    public WishlistService(IRepositoryManager repository, IWishlistAssociations associations)
    {
        _repository = repository;
        _associations = associations;
    }

    public async Task CreateAsync(Wishlist wishlist)
    {
        // create wishlist and save changes
        _repository.Wishlist.Create(wishlist);
        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Wishlist wishlist)
    {
        // delete wishlist and save changes
        _repository.Wishlist.Delete(wishlist);
        await _repository.SaveSafelyAsync();
    }

    public async Task<Wishlist> GetAsync(Guid id)
    {
        var wishlist = await _repository.Wishlist.GetAsync(id) ?? throw new NotFound404Exception(nameof(Wishlist), id);

        return wishlist;
    }

    public async Task<IEnumerable<Wishlist>> IndexAsync()
    {
        var wishlists = await _repository.Wishlist.IndexAsync();

        return wishlists;
    }

    public async Task UpdateAsync(Wishlist wishlist)
    {
        // update wishlist and save changes
        _repository.Wishlist.Update(wishlist);
        await _repository.SaveSafelyAsync();
    }
}
