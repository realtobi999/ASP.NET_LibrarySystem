using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
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

        var bookIds = createWishlistDto.BookIds ?? throw new NullReferenceException("Atleast one book must be assigned.");

        // handle books
        await _associations.AssignBooks(bookIds, wishlist);

        _repository.Wishlist.Create(wishlist);
        await _repository.SaveAsync();

        return wishlist;
    }

    public async Task<int> Delete(Guid id)
    {
        var wishlist = await this.Get(id);

        return await this.Delete(wishlist);
    }

    public async Task<int> Delete(Wishlist wishlist)
    {
        _associations.CleanBooks(wishlist);
        _repository.Wishlist.Delete(wishlist);

        return await _repository.SaveAsync();
    }

    public async Task<Wishlist> Get(Guid id)
    {
        var wishlist = await _repository.Wishlist.Get(id) ?? throw new NotFound404Exception(nameof(Wishlist), id);

        return wishlist;
    }

    public async Task<int> Update(Guid id, UpdateWishlistDto updateWishlistDto)
    {
        var wishlist = await this.Get(id);

        return await this.Update(wishlist, updateWishlistDto);
    }

    public async Task<int> Update(Wishlist wishlist, UpdateWishlistDto updateWishlistDto)
    {
        var name = updateWishlistDto.Name;
        var books = updateWishlistDto.BookIds;

        wishlist.Name = name;

        if (books is not null)
        {
            _associations.CleanBooks(wishlist);
            await _associations.AssignBooks(books, wishlist);
        }

        return await _repository.SaveAsync();
    }
}
