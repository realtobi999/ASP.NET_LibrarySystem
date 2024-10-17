using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class WishlistControllerTests
{
    [Fact]
    public async void CreateWishlist_Returns201AndLocationHeader()
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

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book1.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", await book2.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", await book3.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        // act & assert
        var response = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/wishlist/{wishlist.Id}");
    }

    [Fact]
    public async void GetWishlist_Returns200AndWishlistWithBooksAssigned()
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

        var create5 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/book", await book1.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book2.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", await book3.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create4 = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/wishlist/{wishlist.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<WishlistDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(wishlist.Id);
        content.Books.Count().Should().Be(3);
    }

    [Fact]
    public async void UpdateWishlist_Returns204AndIsUpdated()
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

        var create5 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/book", await book1.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book2.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", await book3.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create4 = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateWishlistDto
        {
            Name = "test_test_test",
            BookIds = [book1.Id],
        };
        var response = await client.PutAsJsonAsync($"/api/wishlist/{wishlist.Id}", updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get1 = await client.GetAsync($"/api/wishlist/{wishlist.Id}");
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get1.Content.ReadFromJsonAsync<WishlistDto>() ?? throw new NullReferenceException();
        
        content.Books.Count().Should().Be(1);
        content.Books.ElementAt(0).Id.Should().Be(book1.Id);
    }

    [Fact]
    public async void DeleteWishlist_Returns204AndIsDeleted()
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

        var create5 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/book", await book1.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book2.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", await book3.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create4 = await client.PostAsJsonAsync("/api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/wishlist/{wishlist.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get1 = await client.GetAsync($"/api/wishlist/{wishlist.Id}");
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
