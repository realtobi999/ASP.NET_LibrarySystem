using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Dtos.Borrows;
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

        var token = JwtTestExtensions.Create().Generate([
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

        var token = JwtTestExtensions.Create().Generate([
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
        content.IsAvailable.Should().Be(false);
    }

    [Fact]
    public async void BorrowController_CreateBorrow_Returns409WhenTryingToBorrowUnavailableBook()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var book = new Book().WithFakeData();
        var borrow1 = new Borrow().WithFakeData(book.Id, user.Id);
        var borrow2 = new Borrow().WithFakeData(book.Id, user.Id);


        var token = JwtTestExtensions.Create().Generate([
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
        content.IsAvailable.Should().Be(false);

        var response = await client.PostAsJsonAsync("/api/borrow", borrow2.ToCreateBorrowDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
    }

    [Fact]
    public async void BorrowController_GetBorrows_Returns200AndLimitOffsetWorks()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var borrow1 = new Borrow().WithFakeData(book1.Id, user.Id);
        var borrow2 = new Borrow().WithFakeData(book2.Id, user.Id);
        var borrow3 = new Borrow().WithFakeData(book3.Id, user.Id);

        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create5 = await client.PostAsJsonAsync("/api/borrow", borrow1.ToCreateBorrowDto());
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/borrow", borrow2.ToCreateBorrowDto());
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/borrow", borrow3.ToCreateBorrowDto());
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync(string.Format("/api/borrow?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BorrowDto>>() ?? throw new DeserializationException();
        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(borrow2.Id);
    }

    [Fact]
    public async void BorrowController_GetBorrows_Returns200AndFilteringByUserIdWorks()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var borrow1 = new Borrow().WithFakeData(book1.Id, user1.Id);
        var borrow2 = new Borrow().WithFakeData(book2.Id, user2.Id);
        var borrow3 = new Borrow().WithFakeData(book3.Id, user2.Id);

        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user1.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user2.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create6 = await client.PostAsJsonAsync("/api/borrow", borrow1.ToCreateBorrowDto());
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/borrow", borrow2.ToCreateBorrowDto());
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create8 = await client.PostAsJsonAsync("/api/borrow", borrow3.ToCreateBorrowDto());
        create8.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/borrow?userId={0}", user1.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BorrowDto>>() ?? throw new DeserializationException();
        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(borrow1.Id);
    }

    [Fact]
    public async void BorrowController_GetBorrow_Returns200AndCorrectBorrowRecord()
    {
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var book = new Book().WithFakeData();
        var borrow = new Borrow().WithFakeData(book.Id, user.Id);

        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/borrow/{0}", borrow.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<BorrowDto>() ?? throw new DeserializationException();
        content.Id.Should().Be(borrow.Id);
    }

    [Fact]
    public async void BorrowController_ReturnBorrow_Returns200AndWorksCorrectly()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var book = new Book().WithFakeData();
        var borrow = new Borrow().WithFakeData(book.Id, user.Id);

        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("/api/borrow", borrow.ToCreateBorrowDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        // act & assert

        // get the book and borrow and check if the availability and returned status is correct
        var get1 = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content1 = await get1.Content.ReadFromJsonAsync<BookDto>() ?? throw new DeserializationException();
        content1.Id.Should().Be(book.Id);
        content1.IsAvailable.Should().Be(false);

        var get2 = await client.GetAsync(string.Format("/api/borrow/{0}", borrow.Id));
        get2.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content2 = await get2.Content.ReadFromJsonAsync<BorrowDto>() ?? throw new DeserializationException();
        content2.Id.Should().Be(borrow.Id);
        content2.Returned.Should().Be(false);

        // return the book
        var response = await client.PutAsJsonAsync(string.Format("/api/borrow/{0}/return", borrow.Id), 0);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // get the book and borrow and check if the availability and returned status is correct
        var get3 = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content3 = await get3.Content.ReadFromJsonAsync<BookDto>() ?? throw new DeserializationException();
        content3.Id.Should().Be(book.Id);
        content3.IsAvailable.Should().Be(true);

        var get4 = await client.GetAsync(string.Format("/api/borrow/{0}", borrow.Id));
        get4.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content4 = await get4.Content.ReadFromJsonAsync<BorrowDto>() ?? throw new DeserializationException();
        content4.Id.Should().Be(borrow.Id);
        content4.Returned.Should().Be(true);
    }
}