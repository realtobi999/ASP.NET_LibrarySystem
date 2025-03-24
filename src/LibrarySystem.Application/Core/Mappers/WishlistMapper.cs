using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;

namespace LibrarySystem.Application.Core.Mappers;

public class WishlistMapper : IMapper<Wishlist, CreateWishlistDto>
{
    private readonly IRepositoryManager _repository;

    public WishlistMapper(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public Wishlist Map(CreateWishlistDto dto)
    {
        var wishlist = new Wishlist
        {
            Id = dto.Id ?? Guid.NewGuid(),
            UserId = dto.UserId,
            Name = dto.Name,
            CreatedAt = DateTimeOffset.UtcNow
        };

        // assign books
        foreach (var bookId in dto.BookIds)
        {
            var book = _repository.Book.Get(bookId);
            if (book is not null)
            {
                wishlist.Books.Add(book);
            }
        }

        return wishlist;
    }
}