using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BorrowControllerTests
{
    [Fact]
    public async void BorrowController_CreateBorrow_Returns201AndLocationHeader()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var book = new Book().WithFakeData();
        var borrow = new Borrow().WithFakeData(book.Id, user.Id);

        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/api/borrow/{0}", borrow.Id));
    }

    [Fact]
    public async void BorrowController_CreateBorrow_BookIsLabeledAsUnavailable()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var book = new Book().WithFakeData();
        var borrow = new Borrow().WithFakeData(book.Id, user.Id);

        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var get = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<BookDto>() ?? throw new DeserializationException();
        content.Id.Should().Be(book.Id);
        content.Available.Should().Be(false);
    }

    [Fact]
    public async void BorrowController_CreateBorrow_Returns409WhenTryingToBorrowUnavailableBook()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var book = new Book().WithFakeData();
        var borrow1 = new Borrow().WithFakeData(book.Id, user.Id);
        var borrow2 = new Borrow().WithFakeData(book.Id, user.Id);


        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var create3 = await client.PostAsJsonAsync("/api/borrow", borrow1.ToCreateBorrowDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var get = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<BookDto>() ?? throw new DeserializationException();
        content.Id.Should().Be(book.Id);
        content.Available.Should().Be(false);

        var response = await client.PostAsJsonAsync("/api/borrow", borrow2.ToCreateBorrowDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
    }
}
