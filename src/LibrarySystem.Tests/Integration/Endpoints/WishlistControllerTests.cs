using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class WishlistControllerTests
{
    [Fact]
    public async void WishlistController_CreateWishlist_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var wishlist = new Wishlist().WithFakeData(user);
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        // act & assert
        var response = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/api/wishlist/{0}", wishlist.Id));
    }

    [Fact]
    public async void WishlistController_GetWishlist_Returns200AndWishlistWithBooksAssigned()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var wishlist = new Wishlist().WithFakeData(user);
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        var create4 = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/wishlist/{0}", wishlist.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);        

        var content = await response.Content.ReadFromJsonAsync<WishlistDto>() ?? throw new NullReferenceException();
        content.Id.Should().Be(wishlist.Id);
        content.Books.Count().Should().Be(3);
    }

    [Fact]
    public async void WishlistController_UpdateWishlist_Returns200AndIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var wishlist = new Wishlist().WithFakeData(user);
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        var create4 = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateWishlistDto
        {
            Name = "test_test_test",
            BookIds = [book1.Id],
        };
        var response = await client.PutAsJsonAsync(string.Format("/api/wishlist/{0}", wishlist.Id), updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get1 = await client.GetAsync(string.Format("/api/wishlist/{0}", wishlist.Id));
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);        

        var content = await get1.Content.ReadFromJsonAsync<WishlistDto>() ?? throw new NullReferenceException();
        content.Books.Count().Should().Be(1);
        content.Books.ElementAt(0).Id.Should().Be(book1.Id);
    }

    [Fact]
    public async void WishlistController_DeleteWishlist_Returns200AndIsDeleted()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var wishlist = new Wishlist().WithFakeData(user);
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        var create4 = await client.PostAsJsonAsync("/api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync(string.Format("/api/wishlist/{0}", wishlist.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get1 = await client.GetAsync(string.Format("/api/wishlist/{0}", wishlist.Id));
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);        
    }
}
