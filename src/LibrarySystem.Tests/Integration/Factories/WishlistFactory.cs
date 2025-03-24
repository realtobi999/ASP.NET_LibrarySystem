using Bogus;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Factories;

internal abstract class WishlistFactory
{
    private static readonly Faker<Wishlist> Factory = new Faker<Wishlist>()
        .RuleFor(w => w.Id, f => f.Random.Guid())
        .RuleFor(w => w.Name, f => f.Lorem.Sentence(3))
        .RuleFor(w => w.CreatedAt, _ => DateTimeOffset.UtcNow);

    public static Wishlist CreateWithFakeData(User user)
    {
        var wishlist = Factory.Generate();
        wishlist.UserId = user.Id;

        return wishlist;
    }
}