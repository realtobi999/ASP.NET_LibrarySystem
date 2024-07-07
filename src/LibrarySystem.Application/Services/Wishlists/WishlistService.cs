using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.NotFound;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;

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

        var bookIds = createWishlistDto.BookIds ?? throw new NullReferenceException("Atleast one book must be assigned.");

        // handle books
        await _associations.AssignBooks(bookIds, wishlist);

        _repository.Wishlist.Create(wishlist);
        await _repository.SaveAsync();

        return wishlist;
    }

    public async Task<Wishlist> Get(Guid id)
    {
        var wishlist = await _repository.Wishlist.Get(id) ?? throw new WishlistNotFoundException(id);

        return wishlist;
    }

    public async Task<int> Update(Guid id, UpdateWishlistDto updateWishlistDto)
    {
        var wishlist = await _repository.Wishlist.Get(id) ?? throw new WishlistNotFoundException(id);

        return await this.Update(wishlist, updateWishlistDto);
    }

    public async Task<int> Update(Wishlist wishlist, UpdateWishlistDto updateWishlistDto)
    {
        var name = updateWishlistDto.Name;
        var bookIds = updateWishlistDto.BookIds;

        if (!name.IsNullOrEmpty())
        {
            wishlist.Name = name;
        }
        if (!bookIds.IsNullOrEmpty())
        {
            _associations.CleanBooks(wishlist);
            
            await _associations.AssignBooks(bookIds!, wishlist);
        }

        return await _repository.SaveAsync();
    }
}
