using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests;

public class BookControllerTests
{
    [Fact]
    public async void BookController_CreateBook_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/api/book/{0}", book.Id));
    }

    [Fact]
    public async void BookController_GetBook_Returns200AndBookWithGenresAndAuthorsAndReviews()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create5 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var user = new User().WithFakeData();

        var create6 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var review = new BookReview().WithFakeData(book, user);
        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        var create7 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();
        content.Id.Should().Be(book.Id);
        content.Authors.Count.Should().Be(2);
        content.Authors.ElementAt(0).Should().BeEquivalentTo(author1.ToDto());
        content.Authors.ElementAt(1).Should().BeEquivalentTo(author2.ToDto());
        content.Genres.Count.Should().Be(2);
        content.Genres.ElementAt(0).Should().BeEquivalentTo(genre1.ToDto());
        content.Genres.ElementAt(1).Should().BeEquivalentTo(genre2.ToDto());
        content.Reviews.Count.Should().Be(1);
        content.Reviews.ElementAt(0).Id.Should().Be(review.Id);
    }

    [Fact]
    public async void BookController_GetBooks_Returns200AndBooks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync(string.Format("/api/book?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();
        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void BookController_UpdateBook_Returns200AndIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var author3 = new Author().WithFakeData();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();
        var genre3 = new Genre().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/genre", genre3.ToCreateGenreDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/author", author3.ToCreateAuthorDto());
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create7 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateBookDto
        {
            Title = "test_test_test",
            Description = "test_test_test",
            PagesCount = 12,
            Authors = [author1.ToDto(), author2.ToDto(), author3.ToDto()],
            Genres = [],
            Availability = false,
            PublishedDate = DateTimeOffset.Now,
        };

        var response = await client.PutAsJsonAsync(string.Format("/api/book/{0}", book.Id), updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();
        content.Id.Should().Be(book.Id);
        content.Title.Should().Be(updateDto.Title);
        content.Description.Should().Be(updateDto.Description);
        content.PublishedDate.Should().Be(updateDto.PublishedDate);
        content.IsAvailable.Should().Be(false);
        content.Authors.Count.Should().Be(3);
        content.Authors.ElementAt(2).Should().BeEquivalentTo(author3.ToDto());
        content.Genres.Count.Should().Be(0);
    }

    [Fact]
    public async void BookController_DeleteBook_Returns200AndIsDeleted()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create5 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync(string.Format("/api/book/{0}", book.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/book/{0}", book.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async void BookController_GetBookByIsbn_Returns200AndCorrectBook()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create5 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/book/isbn/{0}", book.ISBN));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();
        content.Id.Should().Be(book.Id);
        content.ISBN.Should().Be(book.ISBN);
    }

    [Fact]
    public async void BookController_GetBooks_Returns200AndBooksFilteredByAuthor()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author2.Id], [genre1.Id, genre2.Id]));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/book?authorId={0}", author1.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void BookController_GetBooks_Returns200AndBooksFilteredByGenre()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author1.Id, author2.Id], [genre2.Id]));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/book?genreId={0}", genre1.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void BookController_GetBooks_Returns200AndBooksFilteredByGenreAndAuthor()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author2.Id], [genre1.Id]));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author1.Id], [genre1.Id, genre2.Id]));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author1.Id], [genre2.Id]));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/book?genreId={0}&authorId={1}", genre1.Id, author1.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void BookController_SearchBooks_Returns200AndCorrect()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        book1.Title = "test";
        book1.Description = "_";
        book2.Title = "_";
        book2.Description = "test";
        book3.Title = "_";
        book3.Description = "_";

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([], []));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([], []));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([], []));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/book/search/{0}", "tes"));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void BookController_GetEndpoints_WithoutRelationsFilterWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();
        var book3 = new Book().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create5 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author1.Id, author2.Id], [genre1.Id, genre2.Id]));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author2.Id], [genre1.Id, genre2.Id]));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert

        // GET api/book/{book_id} ENDPOINT
        var get1 = await client.GetAsync(string.Format("/api/book/{0}?withRelations=false", book1.Id));
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content1 = await get1.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();
        content1.Id.Should().Be(book1.Id);
        content1.Authors.Count.Should().Be(0);
        content1.Genres.Count.Should().Be(0);

        // GET api/book/isbn/{ISBN} ENDPOINT
        var get2 = await client.GetAsync(string.Format("/api/book/isbn/{0}?withRelations=false", book1.ISBN));
        get2.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content2 = await get2.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();
        content2.ISBN.Should().Be(book1.ISBN);
        content2.Authors.Count.Should().Be(0);
        content2.Genres.Count.Should().Be(0);

        // GET api/book ENDPOINT
        var get3 = await client.GetAsync("/api/book?withRelations=false&limit=1");
        get3.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content3 = await get3.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();
        content3.Count.Should().Be(1);
        content3.ElementAt(0).Id.Should().Be(book1.Id);
        content3.ElementAt(0).Authors.Count.Should().Be(0);
        content3.ElementAt(0).Genres.Count.Should().Be(0);
    }
}
