using Bogus;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Extensions;

public static class WishlistTestExtensions
{
    private static readonly Faker<Wishlist> _WishlistFaker = new Faker<Wishlist>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.Name, f => f.Lorem.Sentence(3));

    public static Wishlist WithFakeData(this Wishlist wishlist, User user)
    {
        var fakeWishlist = _WishlistFaker.Generate();
        fakeWishlist.UserId = user.Id;

        return fakeWishlist;
    }

    public static CreateWishlistDto ToCreateWishlistDto(this Wishlist wishlist, IEnumerable<Guid> bookIds)
    {
        return new CreateWishlistDto
        {
            Id = wishlist.Id,
            UserId = wishlist.UserId,
            Name = wishlist.Name,
            BookIds = bookIds
        };
    }
}
