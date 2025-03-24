using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Wishlists;

public class WishlistService : IWishlistService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Wishlist> _validator;

    public WishlistService(IRepositoryManager repository, IValidator<Wishlist> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task CreateAsync(Wishlist wishlist)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(wishlist);
        if (!valid && exception is not null) throw exception;

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
        // validate
        var (valid, exception) = await _validator.ValidateAsync(wishlist);
        if (!valid && exception is not null) throw exception;

        // update wishlist and save changes
        _repository.Wishlist.Update(wishlist);
        await _repository.SaveSafelyAsync();
    }
}