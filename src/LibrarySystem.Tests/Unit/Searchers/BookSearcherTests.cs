using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Searchers;

public class BookSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly Mock<ISearcher<Genre>> _genreSearcher;
    private readonly Mock<ISearcher<Author>> _authorSearcher;
    private readonly ISearcher<Book> _searcher;
    private static readonly Random Random = new();

    public BookSearcherTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _authorSearcher = new Mock<ISearcher<Author>>();
        _genreSearcher = new Mock<ISearcher<Genre>>();

        _searcher = new BookSearcher(_repository.Object, _genreSearcher.Object, _authorSearcher.Object);
    }

    [Fact]
    public async Task SearchAsync_ShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (var i = 0; i < 100; i++)
        {
            books.Add(BookFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random books from the list
        var bookToSearchFor1 = books[Random.Next(books.Count)];
        var bookToSearchFor2 = books[Random.Next(books.Count)];
        bookToSearchFor1.Title = $"{keyword} test";
        bookToSearchFor2.Description = $"test {keyword}";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync(keyword);

        searchedBooks.Count().Should().Be(2);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor2).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (var i = 0; i < 100; i++)
        {
            books.Add(BookFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random books from the list
        var bookToSearchFor1 = books[Random.Next(books.Count)];
        var bookToSearchFor2 = books[Random.Next(books.Count)];
        bookToSearchFor1.Title = $"{keyword} test";
        bookToSearchFor2.Description = $"test {keyword}";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync($"\t  {keyword}         \n ");

        searchedBooks.Count().Should().Be(2);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor2).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (var i = 0; i < 100; i++)
        {
            books.Add(BookFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random books from the list
        var bookToSearchFor1 = books[Random.Next(books.Count)];
        var bookToSearchFor2 = books[Random.Next(books.Count)];
        bookToSearchFor1.Title = $"{keyword} test";
        bookToSearchFor2.Description = $"test {keyword}";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync($"{BookSearcher.FILTER_OPERATOR}{keyword}");

        searchedBooks.Count().Should().Be(1);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (var i = 0; i < 100; i++)
        {
            books.Add(BookFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random books from the list
        var bookToSearchFor1 = books[Random.Next(books.Count)];
        var bookToSearchFor2 = books[Random.Next(books.Count)];
        bookToSearchFor1.Title = $"test {keyword}";
        bookToSearchFor2.Description = $"{keyword} test";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync($"{keyword}{BookSearcher.FILTER_OPERATOR}");

        searchedBooks.Count().Should().Be(1);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
    }


    [Fact]
    public async Task SearchAsync_ShouldWorkWithAuthorAngGenreQuerySearch()
    {
        // prepare
        var books = new List<Book>();
        var genre = GenreFactory.CreateWithFakeData();
        var author = AuthorFactory.CreateWithFakeData();

        var keyword = Guid.NewGuid().ToString();

        // change both genre and authors name so we can find by them
        genre.Name = keyword;
        author.Name = keyword;

        for (var i = 0; i < 100; i++)
        {
            books.Add(BookFactory.CreateWithFakeData());
        }

        // get random books from the list
        var bookToSearchFor1 = books[Random.Next(books.Count)];
        var bookToSearchFor2 = books[Random.Next(books.Count)];
        var bookToSearchFor3 = books[Random.Next(books.Count)];
        bookToSearchFor1.Genres = [genre];
        bookToSearchFor2.Authors = [author];
        bookToSearchFor3.Title = keyword;

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));
        _genreSearcher.Setup(gs => gs.SearchAsync(keyword)).ReturnsAsync([genre]);
        _authorSearcher.Setup(gs => gs.SearchAsync(keyword)).ReturnsAsync([author]);

        // act & assert
        var searchedBooks = await _searcher.SearchAsync(keyword);

        searchedBooks.Count().Should().Be(3);
        searchedBooks.Contains(bookToSearchFor1).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor2).Should().BeTrue();
        searchedBooks.Contains(bookToSearchFor3).Should().BeTrue();
    }
}