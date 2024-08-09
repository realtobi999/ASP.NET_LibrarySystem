using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Dtos.Messages;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BookReviewControllerTests
{
    [Fact]
    public async void BookReviewController_CreateBookReview_Returns201()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var user = new User().WithFakeData();
        var review = new BookReview().WithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        // act & assert
        var response = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Fact]
    public async void BookReviewController_DeleteBookReview_Returns200()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var user = new User().WithFakeData();
        var review = new BookReview().WithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        var create3 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync(string.Format("/api/review/{0}", review.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async void BookReviewController_DeleteBookReview_Returns401WhenTryingToDeleteOtherUserReview()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var user = new User().WithFakeData();
        var review = new BookReview().WithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        var create3 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var token3 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", Guid.NewGuid().ToString()), // insert invalid user id
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token3));

        var response = await client.DeleteAsync(string.Format("/api/review/{0}", review.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void BookReviewController_UpdateBookReview_Returns200AndTextIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var user = new User().WithFakeData();
        var review = new BookReview().WithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        var create3 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateBookReviewDto = new UpdateBookReviewDto
        {
            Text = "Test_Test_Test"
        };

        var response = await client.PutAsJsonAsync(string.Format("/api/review/{0}", review.Id), updateBookReviewDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();
        content.Id.Should().Be(book.Id);
        content.Reviews.Count.Should().Be(1);
        content.Reviews.ElementAt(0).Id.Should().Be(review.Id);
        content.Reviews.ElementAt(0).Text.Should().Be(updateBookReviewDto.Text);
    }
}