using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;

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

    public async Task<Wishlist> Create(CreateWishlistDto createWishlistDto)
    {
        var wishlist = new Wishlist
        {
            Id = createWishlistDto.Id ?? Guid.NewGuid(),
            UserId = createWishlistDto.UserId,
            Name = createWishlistDto.Name
        };

        var bookIds = createWishlistDto.BookIds ?? throw new ArgumentNullException("Atleast one book must be assigned.");

        // handle books
        await _associations.AssignBooksAsync(bookIds, wishlist);

        _repository.Wishlist.Create(wishlist);
        await _repository.SaveAsync();

        return wishlist;
    }

    public async Task<Wishlist> Get(Guid id)
    {
        var wishlist = await _repository.Wishlist.Get(id) ?? throw new WishlistNotFoundException(id);

        return wishlist;
    }
}
