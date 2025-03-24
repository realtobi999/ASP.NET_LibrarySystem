using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BookControllerTests
{
    [Fact]
    public async Task GetBooks_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        const int limit = 2;
        const int offset = 1;

        var response = await client.GetAsync($"/api/book?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async Task GetBooks_Returns200AndCorrectValuesFilteredByAuthor()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author1 = AuthorFactory.CreateWithFakeData();
        var author2 = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author1.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author1.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author2.Id], [genre.Id])); // we assign the second author
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book?authorId={author1.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async Task GetBooks_Returns200AndCorrectValuesFilteredByGenre()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre1.Id, genre2.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre1.Id, genre2.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre2.Id])); // we assign the second genre
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book?genreId={genre1.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async Task GetBooks_Returns200AndCorrectValuesFilteredRecently()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // update the CreatedAt property in the database for more accurate assertions
        await using var context = app.GetDatabaseContext();

        var ids = new[] { book1.Id, book2.Id, book3.Id };
        for (var i = 0; i < ids.Length; i++)
        {
            var book = context.Set<Book>().FirstOrDefault(b => b.Id == ids[i]) ?? throw new NullReferenceException();

            book.CreatedAt = DateTimeOffset.Now.AddDays(-(i + 1));
        }

        await context.SaveChangesAsync();

        // act & assert
        var response = await client.GetAsync("/api/book/recent");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(3);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
        content.ElementAt(2).Id.Should().Be(book3.Id);
    }

    [Fact]
    public async Task GetBook_Returns200AndCorrectValue()
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

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // auth as user to create the review
        client.DefaultRequestHeaders.Remove("Authorization");
        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create4 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book/{book.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(book.Id);
    }

    [Fact]
    public async Task GetBook_Returns200AndCorrectValueGotByISBN()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book/isbn/{book.Isbn}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(book.Id);
    }

    [Fact]
    public async Task CreateBook_Returns201AndBookIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/book/{book.Id}");

        // assert that the book is created
        await using var context = app.GetDatabaseContext();
        context.Set<Book>().Any(b => b.Id == book.Id).Should().BeTrue();
    }


    [Fact]
    public async Task UpdateBook_Returns204AndBookIsUpdated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author1 = AuthorFactory.CreateWithFakeData();
        var author2 = AuthorFactory.CreateWithFakeData();
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author1.Id], [genre1.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateBookDto
        {
            Title = "test_test_test",
            Description = "test_test_test",
            PagesCount = 12,
            AuthorIds = [author2.Id],
            GenreIds = [genre2.Id],
            Availability = false,
            PublishedDate = DateTimeOffset.Now
        };

        var response = await client.PutAsJsonAsync($"/api/book/{book.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the book is updated
        await using var context = app.GetDatabaseContext();
        var updatedBook = context.Set<Book>().Include(b => b.Authors).Include(b => b.Genres).FirstOrDefault(b => b.Id == book.Id) ??
                          throw new NullReferenceException();

        updatedBook.Title.Should().Be(updateDto.Title);
        updatedBook.Description.Should().Be(updateDto.Description);
        updatedBook.PagesCount.Should().Be(updateDto.PagesCount);
        updatedBook.Authors.Count.Should().Be(1);
        updatedBook.Authors.ElementAt(0).Id.Should().Be(author2.Id);
        updatedBook.Genres.Count.Should().Be(1);
        updatedBook.Genres.ElementAt(0).Id.Should().Be(genre2.Id);
        updatedBook.IsAvailable.Should().Be(false);
        updatedBook.PublishedDate.Should().Be(updateDto.PublishedDate);
    }

    [Fact]
    public async Task DeleteBook_Returns204AndBookIsDeleted()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/book/{book.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the book is created
        await using var context = app.GetDatabaseContext();
        context.Set<Book>().Any(b => b.Id == book.Id).Should().BeFalse();
    }

    [Fact]
    public async Task SearchBooks_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        book1.Title = "test";
        book1.Description = "_";
        book2.Title = "_";
        book2.Description = "test";
        book3.Title = "_";
        book3.Description = "_";

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book1.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("/api/book", book2.ToCreateBookDto([author.Id], [genre.Id]));
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("/api/book", book3.ToCreateBookDto([author.Id], [genre.Id]));
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        const string query = "tes";
        var response = await client.GetAsync($"/api/book/search/{query}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async Task UploadPhotos_Returns204AndPhotoUploaded()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author = AuthorFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([author.Id], [genre.Id]));
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // create photo files and then assign them
        var photo1 = new ByteArrayContent([1, 2, 3, 4]);
        photo1.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        var photo2 = new ByteArrayContent([5, 6, 7, 8]);
        photo2.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        var formData = new MultipartFormDataContent
        {
            { photo1, "files", "photo1.jpg" },
            { photo2, "files", "photo2.jpg" }
        };

        // act & assert
        var response = await client.PutAsync($"/api/book/{book.Id}/photo", formData);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the book cover picture is uploaded
        await using var context = app.GetDatabaseContext();
        var updatedBook = context.Set<Book>().Include(b => b.CoverPictures).FirstOrDefault(b => b.Id == book.Id) ??
                          throw new NullReferenceException();

        updatedBook.CoverPictures.Should().NotBeNullOrEmpty();
        updatedBook.CoverPictures.Count.Should().Be(2);
    }
}