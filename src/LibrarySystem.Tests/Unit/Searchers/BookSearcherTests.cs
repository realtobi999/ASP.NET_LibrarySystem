using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Searchers;

public class BookSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly Mock<ISearcher<Genre>> _genreSearcher;
    private readonly Mock<ISearcher<Author>> _authorSearcher;
    private readonly ISearcher<Book> _searcher;
    private static readonly Random _random = new();

    public BookSearcherTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _authorSearcher = new Mock<ISearcher<Author>>();
        _genreSearcher = new Mock<ISearcher<Genre>>();

        _searcher = new BookSearcher(_repository.Object, _genreSearcher.Object, _authorSearcher.Object);
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(BookFactory.CreateWithFakeData());
        }

        // get random books from the list
        var bookToSearchFor1 = books[_random.Next(books.Count)];
        var bookToSearchFor2 = books[_random.Next(books.Count)];
        bookToSearchFor1.Title = "testing";
        bookToSearchFor2.Description = "test";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync("test");

        searchedBooks.Count().Should().Be(2);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor2).Should().BeTrue();
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(BookFactory.CreateWithFakeData());
        }

        // get random books from the list
        var bookToSearchFor1 = books[_random.Next(books.Count)];
        var bookToSearchFor2 = books[_random.Next(books.Count)];
        bookToSearchFor1.Title = "test not so";
        bookToSearchFor2.Description = "test not so";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync("\t test not so          \n ");

        searchedBooks.Count().Should().Be(2);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor2).Should().BeTrue();
    }

    [Fact]
    public async void SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(BookFactory.CreateWithFakeData());
        }

        // get random books from the list
        var bookToSearchFor1 = books[_random.Next(books.Count)];
        var bookToSearchFor2 = books[_random.Next(books.Count)];
        bookToSearchFor1.Title = "test not so";
        bookToSearchFor2.Description = "not so test";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync($"{BookSearcher.FILTER_OPERATOR}test");

        searchedBooks.Count().Should().Be(1);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
    }

    [Fact]
    public async void SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(BookFactory.CreateWithFakeData());
        }

        // get random books from the list
        var bookToSearchFor1 = books[_random.Next(books.Count)];
        var bookToSearchFor2 = books[_random.Next(books.Count)];
        bookToSearchFor1.Title = "test not so";
        bookToSearchFor2.Description = "not so test";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync($"so{BookSearcher.FILTER_OPERATOR}");

        searchedBooks.Count().Should().Be(1);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
    }


    [Fact]
    public async void SearchAsync_ShouldWorkWithAuthorAngGenreQuerySearch()
    {
        // prepare
        var books = new List<Book>();
        var genre = GenreFactory.CreateWithFakeData();
        var author = AuthorFactory.CreateWithFakeData();

        var query = "test";

        // change both genre and authors name so we can find by them
        genre.Name = query;
        author.Name = query;

        for (int i = 0; i < 100; i++)
        {

            books.Add(BookFactory.CreateWithFakeData());
        }

        // get random books from the list
        var bookToSearchFor1 = books[_random.Next(books.Count)];
        var bookToSearchFor2 = books[_random.Next(books.Count)];
        var bookToSearchFor3 = books[_random.Next(books.Count)];
        bookToSearchFor1.Genres = [genre];
        bookToSearchFor2.Authors = [author];
        bookToSearchFor3.Title = query;

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));
        _genreSearcher.Setup(gs => gs.SearchAsync(query)).ReturnsAsync([genre]);
        _authorSearcher.Setup(gs => gs.SearchAsync(query)).ReturnsAsync([author]);

        // act & assert
        var searchedBooks = await _searcher.SearchAsync(query);

        searchedBooks.Count().Should().Be(3);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor2).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor3).Should().BeTrue();
    }
}
