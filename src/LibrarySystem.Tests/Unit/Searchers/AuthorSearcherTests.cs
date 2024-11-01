using LibrarySystem.Application.Services.Authors;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Helpers;
using Moq;

namespace LibrarySystem.Tests.Unit.Searchers;

public class AuthorSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly ISearcher<Author> _searcher;
    private static readonly Random _random = new Random();

    public AuthorSearcherTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _searcher = new AuthorSearcher(_repository.Object);
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (int i = 0; i < 100; i++)
        {
            authors.Add(new Author().WithFakeData());
        }

        var authorToSearchFor1 = authors[_random.Next(authors.Count) - 1];
        authorToSearchFor1.Name = "testing";
        var authorToSearchFor2 = authors[authors.IndexOf(authorToSearchFor1) + 1];
        authorToSearchFor2.Name = "test author";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors);

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync("test");

        searchedAuthors.Count().Should().Be(2);
        searchedAuthors.ElementAt(0).Id.Should().Be(authorToSearchFor1.Id);
        searchedAuthors.ElementAt(1).Id.Should().Be(authorToSearchFor2.Id);
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (int i = 0; i < 100; i++)
        {
            authors.Add(new Author().WithFakeData());
        }

        var authorToSearchFor1 = authors[_random.Next(authors.Count)];
        authorToSearchFor1.Name = "test not so";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync("\t test not so          \n ");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.ElementAt(0).Id.Should().Be(authorToSearchFor1.Id);
    }

    [Fact]
    public async void SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (int i = 0; i < 100; i++)
        {
            authors.Add(new Author().WithFakeData());
        }

        var authorToSearchFor1 = authors[_random.Next(authors.Count + 1)];
        authorToSearchFor1.Name = "test not so";
        var authorToSearchFor2 = authors[_random.Next(authors.Count - 1)];
        authorToSearchFor2.Name = "not so test";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync($"{Constants.QUERY_SEARCH_FILTER_OPERATOR}test");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.ElementAt(0).Id.Should().Be(authorToSearchFor1.Id);
    }

    [Fact]
    public async void SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (int i = 0; i < 100; i++)
        {
            authors.Add(new Author().WithFakeData());
        }

        var authorToSearchFor1 = authors[_random.Next(authors.Count - 1)];
        authorToSearchFor1.Name = "not so test";
        var authorToSearchFor2 = authors[_random.Next(authors.Count + 1)];
        authorToSearchFor2.Name = "test not so";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync($"test{Constants.QUERY_SEARCH_FILTER_OPERATOR}");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.ElementAt(0).Id.Should().Be(authorToSearchFor1.Id);
    }
}
