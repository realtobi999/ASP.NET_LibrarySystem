using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal class WishlistFactory
{
    private static readonly Faker<Wishlist> _factory = new Faker<Wishlist>()
        .RuleFor(b => b.Id, f => f.Random.Guid())
        .RuleFor(b => b.Name, f => f.Lorem.Sentence(3));

    public static Wishlist CreateWithFakeData(User user)
    {
        var wishlist = _factory.Generate();
        wishlist.UserId = user.Id;

        return wishlist;
    }
}
