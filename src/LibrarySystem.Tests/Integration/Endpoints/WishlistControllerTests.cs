using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class WishlistControllerTests
{
    [Fact]
    public async Task GetWishlist_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var wishlist = WishlistFactory.CreateWithFakeData(user);
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        var create7 = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create7.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/wishlist/{wishlist.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<WishlistDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(wishlist.Id);
        content.Books.Count().Should().Be(3);
    }

    [Fact]
    public async Task CreateWishlist_Returns201AndWishlistIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var wishlist = WishlistFactory.CreateWithFakeData(user);
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        // act & assert
        var response = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/wishlist/{wishlist.Id}");

        // assert that the wishlist is created
        await using var context = app.GetDatabaseContext();
        context.Set<Wishlist>().Any(w => w.Id == wishlist.Id).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateWishlist_Returns204AndIsWishlistUpdated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var wishlist = WishlistFactory.CreateWithFakeData(user);
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        var create7 = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create7.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateWishlistDto
        {
            Name = "test_test_test",
            BookIds = [book1.Id]
        };

        var response = await client.PutAsJsonAsync($"/api/wishlist/{wishlist.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the wishlist is updated
        await using var context = app.GetDatabaseContext();
        var content = context.Set<Wishlist>().Include(w => w.Books).FirstOrDefault(w => w.Id == wishlist.Id) ?? throw new NullReferenceException();

        content.Books.Count.Should().Be(1);
        content.Books.ElementAt(0).Id.Should().Be(book1.Id);
    }

    [Fact]
    public async Task DeleteWishlist_Returns204AndWishlistIsDeleted()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var wishlist = WishlistFactory.CreateWithFakeData(user);
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        var create7 = await client.PostAsJsonAsync("api/wishlist", wishlist.ToCreateWishlistDto([book1.Id, book2.Id, book3.Id]));
        create7.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/wishlist/{wishlist.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the wishlist is deleted
        await using var context = app.GetDatabaseContext();
        context.Set<Wishlist>().Any(w => w.Id == wishlist.Id).Should().BeFalse();
    }
}