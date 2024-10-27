using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Helpers;
using Moq;

namespace LibrarySystem.Tests.Unit.Books;

public class BookSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly ISearcher<Book> _searcher;
    private static readonly Random _random = new Random();

    public BookSearcherTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _searcher = new BookSearcher(_repository.Object);
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(new Book().WithFakeData());
        }

        var bookToSearchFor1 = books[_random.Next(books.Count - 1)];
        bookToSearchFor1.Title = "testing";
        var bookToSearchFor2 = books[books.IndexOf(bookToSearchFor1) + 1];
        bookToSearchFor2.Description = "test";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync("test");

        searchedBooks.Count().Should().Be(2);
        searchedBooks.ElementAt(0).Id.Should().Be(bookToSearchFor1.Id);
        searchedBooks.ElementAt(1).Id.Should().Be(bookToSearchFor2.Id);
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(new Book().WithFakeData());
        }

        var bookToSearchFor1 = books[_random.Next(books.Count - 1)];
        bookToSearchFor1.Title = "test not so";
        var bookToSearchFor2 = books[books.IndexOf(bookToSearchFor1) + 1];
        bookToSearchFor2.Description = "test not so";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync("\t test not so          \n ");

        searchedBooks.Count().Should().Be(2);
        searchedBooks.ElementAt(0).Id.Should().Be(bookToSearchFor1.Id);
        searchedBooks.ElementAt(1).Id.Should().Be(bookToSearchFor2.Id);
    }

    [Fact]
    public async void SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(new Book().WithFakeData());
        }

        var bookToSearchFor1 = books[_random.Next(books.Count - 1)];
        bookToSearchFor1.Title = "test not so";
        var bookToSearchFor2 = books[books.IndexOf(bookToSearchFor1) + 1];
        bookToSearchFor2.Description = "not so test";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync($"%test");

        searchedBooks.Count().Should().Be(1);
        searchedBooks.ElementAt(0).Id.Should().Be(bookToSearchFor1.Id);
    }

    [Fact]
    public async void SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var books = new List<Book>();

        for (int i = 0; i < 100; i++)
        {

            books.Add(new Book().WithFakeData());
        }

        var bookToSearchFor1 = books[_random.Next(books.Count - 1)];
        bookToSearchFor1.Title = "test not so";
        var bookToSearchFor2 = books[books.IndexOf(bookToSearchFor1) + 1];
        bookToSearchFor2.Description = "not so test";

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books.OrderBy(b => b.CreatedAt));

        // act & assert
        var searchedBooks = await _searcher.SearchAsync($"so%");

        searchedBooks.Count().Should().Be(1);
        searchedBooks.ElementAt(0).Id.Should().Be(bookToSearchFor1.Id);
    }
}
