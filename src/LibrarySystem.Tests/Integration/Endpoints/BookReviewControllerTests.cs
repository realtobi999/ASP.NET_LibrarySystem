using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BookReviewControllerTests
{
    [Fact]
    public async Task CreateBookReview_Returns201AndBookReviewIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);

        // auth as the user to create the review
        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        // act & assert
        var response = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // assert that the book review is created
        await using var context = app.GetDatabaseContext();
        context.Set<BookReview>().Any(r => r.Id == review.Id).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateBookReview_Returns204AndBookReviewIsUpdated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);

        // auth as the user to create the review
        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");


        var create5 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateBookReviewDto
        {
            Text = "Test_Test_Test",
            Rating = review.Rating
        };

        var response = await client.PutAsJsonAsync($"/api/review/{review.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the book review is created
        await using var context = app.GetDatabaseContext();
        var updatedReview = context.Set<BookReview>().FirstOrDefault(r => r.Id == review.Id) ?? throw new NullReferenceException();

        updatedReview.Id.Should().Be(review.Id);
        updatedReview.Text.Should().Be(updateDto.Text);
    }

    [Fact]
    public async Task DeleteBookReview_Returns204AndBookReviewIsDeleted()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);
        var employeeToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {employeeToken}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);

        // auth as the user to create the review
        client.DefaultRequestHeaders.Remove("Authorization");
        var userToken = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        var create5 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/review/{review.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the book review is deleted
        await using var context = app.GetDatabaseContext();
        context.Set<BookReview>().Any(r => r.Id == review.Id).Should().BeFalse();
    }
}