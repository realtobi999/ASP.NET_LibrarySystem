using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BookControllerTests
{
    [Fact]
    public async void CreateBook_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
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
        header.Should().Equal($"/api/book/{book.Id}");
    }

    [Fact]
    public async void GetBook_Returns200AndBookWithGenresAndAuthorsAndReviews()
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

        var author1 = AuthorFactory.CreateWithFakeData();
        var author2 = AuthorFactory.CreateWithFakeData();
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();

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
        var create6 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // auth as user to create the review
        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        var create7 = await client.PostAsJsonAsync("/api/review", review.ToCreateBookReviewDto());
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book/{book.Id}");
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
    public async void GetBooks_Returns200AndBooks()
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

        var create5 = await client.PostAsJsonAsync("/api/book", await book1.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", await book2.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", await book3.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync($"/api/book?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void UpdateBook_Returns204AndIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var author1 = AuthorFactory.CreateWithFakeData();
        var author2 = AuthorFactory.CreateWithFakeData();
        var author3 = AuthorFactory.CreateWithFakeData();
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();
        var genre3 = GenreFactory.CreateWithFakeData();

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
            AuthorIds = [author1.Id, author2.Id, author3.Id],
            GenreIds = [genre3.Id],
            Availability = false,
            PublishedDate = DateTimeOffset.Now,
        };

        var response = await client.PutAsJsonAsync($"/api/book/{book.Id}", updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get = await client.GetAsync($"/api/book/{book.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(book.Id);
        content.Title.Should().Be(updateDto.Title);
        content.Description.Should().Be(updateDto.Description);
        content.PublishedDate.Should().Be(updateDto.PublishedDate);
        content.IsAvailable.Should().Be(false);
        content.Authors.Count.Should().Be(3);
        content.Authors.ElementAt(2).Id.Should().Be(author3.Id);
        content.Genres.Count.Should().Be(1);
        content.Genres.ElementAt(0).Id.Should().Be(genre3.Id);
    }

    [Fact]
    public async void DeleteBook_Returns204AndIsDeleted()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/book/{book.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get = await client.GetAsync($"/api/book/{book.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async void GetBookByIsbn_Returns200AndCorrectBook()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book/isbn/{book.ISBN}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(book.Id);
        content.ISBN.Should().Be(book.ISBN);
    }

    [Fact]
    public async void GetBooks_Returns200AndBooksFilteredByAuthor()
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
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();

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
        var response = await client.GetAsync($"/api/book?authorId={author1.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void GetBooks_Returns200AndBooksFilteredByGenre()
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
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();

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
        var response = await client.GetAsync($"/api/book?genreId={genre1.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void GetBooks_Returns200AndBooksFilteredByGenreAndAuthor()
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
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();

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
        var response = await client.GetAsync($"/api/book?genreId={genre1.Id}&authorId={author1.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void SearchBooks_Returns200AndCorrect()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();
        var book3 = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        book1.Title = "test";
        book1.Description = "_";
        book2.Title = "_";
        book2.Description = "test";
        book3.Title = "_";
        book3.Description = "_";

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create5 = await client.PostAsJsonAsync("/api/book", await book1.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", await book2.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", await book3.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book/search/{"tes"}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
    }

    [Fact]
    public async void UploadPhotos_Returns204AndPhotosUploaded()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = BookFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/book", await book.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get = await client.GetAsync($"/api/book/{book.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<BookDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(book.Id);
        content.CoverPictures.Should().NotBeNullOrEmpty();
        content.CoverPictures!.Count.Should().Be(2);
    }

    [Fact]
    public async void GetBooks_Returns200AndMostRecentBooks()
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

        var create5 = await client.PostAsJsonAsync("/api/book", await book1.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create5.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("/api/book", await book2.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create6.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("/api/book", await book3.ToCreateBookDtoWithGenresAndAuthorsAsync(client));
        create7.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/book/recent");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<BookDto>>() ?? throw new NullReferenceException();

        content.ElementAt(0).Id.Should().Be(book1.Id);
        content.ElementAt(1).Id.Should().Be(book2.Id);
        content.ElementAt(2).Id.Should().Be(book3.Id);
    }
}
