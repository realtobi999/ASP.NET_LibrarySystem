using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BookReviewControllerTests
{
    [Fact]
    public async void CreateBookReview_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // auth as the user to create the review
        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        // act & assert
        var response = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async void DeleteBookReview_Returns204()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // auth as the user to create the review
        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create3 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/review/{review.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }

    [Fact]
    public async void DeleteBookReview_Returns401WhenTryingToDeleteOtherUserReview()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // auth as the user to create the review
        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create3 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var token3 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", Guid.NewGuid().ToString()), // insert invalid user id
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token3}");

        var response = await client.DeleteAsync($"/api/review/{review.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void UpdateBookReview_Returns204AndTextIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // auth as the user to create the review
        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create3 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateBookReviewDto = new UpdateBookReviewDto
        {
            Text = "Test_Test_Test",
            Rating = review.Rating
        };

        var response = await client.PutAsJsonAsync($"/api/review/{review.Id}", updateBookReviewDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get = await client.GetAsync($"/api/book/{book.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();
        content.Id.Should().Be(book.Id);
        content.Reviews.Count.Should().Be(1);
        content.Reviews.ElementAt(0).Id.Should().Be(review.Id);
        content.Reviews.ElementAt(0).Text.Should().Be(updateBookReviewDto.Text);
    }
}
