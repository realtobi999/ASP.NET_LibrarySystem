using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BorrowControllerTests
{
    [Fact]
    public async void GetBorrows_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var borrow1 = BorrowFactory.CreateWithFakeData(book1, user);
        var borrow2 = BorrowFactory.CreateWithFakeData(book2, user);
        var borrow3 = BorrowFactory.CreateWithFakeData(book3, user);
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/borrow", borrow1.ToCreateBorrowDto());
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create8 = await client.PostAsJsonAsync("/api/borrow", borrow2.ToCreateBorrowDto());
        create8.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create9 = await client.PostAsJsonAsync("/api/borrow", borrow3.ToCreateBorrowDto());
        create9.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync($"/api/borrow?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BorrowDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(borrow2.Id);
    }

    [Fact]
    public async void GetBorrows_Returns200AndCorrectValuesFilteredByUser()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = UserFactory.CreateWithFakeData();
        var user2 = UserFactory.CreateWithFakeData();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var borrow1 = BorrowFactory.CreateWithFakeData(book1, user1);
        var borrow2 = BorrowFactory.CreateWithFakeData(book2, user2);
        var borrow3 = BorrowFactory.CreateWithFakeData(book3, user2);
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user1.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user2.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create8 = await client.PostAsJsonAsync("/api/borrow", borrow1.ToCreateBorrowDto());
        create8.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create9 = await client.PostAsJsonAsync("/api/borrow", borrow2.ToCreateBorrowDto());
        create9.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create10 = await client.PostAsJsonAsync("/api/borrow", borrow3.ToCreateBorrowDto());
        create10.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/borrow?userId={user1.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BorrowDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(borrow1.Id);
    }

    [Fact]
    public async void GetBorrow_Returns200AndCorrectValue()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/borrow/{borrow.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<BorrowDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(borrow.Id);
    }

    [Fact]
    public async void CreateBorrow_Returns201AndBorrowIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/borrow/{borrow.Id}");

        // assert that the borrow is created
        using var context = app.GetDatabaseContext();
        context.Set<Borrow>().Any(b => b.Id == borrow.Id).Should().BeTrue();
    }

    [Fact]
    public async void CreateBorrow_Returns201AndBookIsUnavailable()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // assert that the book is unavailable
        using var context = app.GetDatabaseContext();
        var updatedBook = context.Set<Book>().FirstOrDefault(b => b.Id == book.Id) ?? throw new NullReferenceException();

        updatedBook.Id.Should().Be(book.Id);
        updatedBook.IsAvailable.Should().BeFalse();
    }

    [Fact]
    public async void CreateBorrow_Returns409WhenTryingToBorrowUnavailableBook()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow1 = BorrowFactory.CreateWithFakeData(book, user);
        var borrow2 = BorrowFactory.CreateWithFakeData(book, user);
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var create3 = await client.PostAsJsonAsync("/api/borrow", borrow1.ToCreateBorrowDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // assert that the book is unavailable
        using var context = app.GetDatabaseContext();
        var updatedBook = context.Set<Book>().FirstOrDefault(b => b.Id == book.Id) ?? throw new NullReferenceException();

        updatedBook.Id.Should().Be(book.Id);
        updatedBook.IsAvailable.Should().BeFalse();

        var response = await client.PostAsJsonAsync("/api/borrow", borrow2.ToCreateBorrowDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
    }


    [Fact]
    public async void ReturnBorrow_Returns204AndBorrowIsReturned()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        // act & assert

        // get the book and borrow and check if the availability and IsReturned status is correct
        using (var context = app.GetDatabaseContext())
        {
            var updatedBook = context.Set<Book>().FirstOrDefault(b => b.Id == book.Id) ?? throw new NullReferenceException();

            updatedBook.Id.Should().Be(book.Id);
            updatedBook.IsAvailable.Should().BeFalse();

            var updatedBorrow = context.Set<Borrow>().FirstOrDefault(b => b.Id == borrow.Id) ?? throw new NullReferenceException();

            updatedBorrow.Id.Should().Be(borrow.Id);
            updatedBorrow.IsReturned.Should().Be(false);
        }

        // return the book
        var response = await client.PutAsJsonAsync($"/api/borrow/{borrow.Id}/return", 0);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // get the book and borrow and check if the availability and IsReturned status is correct
        using (var context = app.GetDatabaseContext())
        {
            var updatedBook = context.Set<Book>().FirstOrDefault(b => b.Id == book.Id) ?? throw new NullReferenceException();

            updatedBook.Id.Should().Be(book.Id);
            updatedBook.IsAvailable.Should().Be(true);

            var updatedBorrow = context.Set<Borrow>().FirstOrDefault(b => b.Id == borrow.Id) ?? throw new NullReferenceException();

            updatedBorrow.Id.Should().Be(borrow.Id);
            updatedBorrow.IsReturned.Should().Be(true);
        }
    }
}