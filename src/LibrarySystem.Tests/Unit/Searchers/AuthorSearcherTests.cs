using LibrarySystem.Application.Services.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Searchers;

public class AuthorSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly ISearcher<Author> _searcher;
    private static readonly Random _random = new();

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
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        // get random authors from the list
        var authorToSearchFor1 = authors[_random.Next(authors.Count)];
        var authorToSearchFor2 = authors[_random.Next(authors.Count)];
        authorToSearchFor1.Name = "testing";
        authorToSearchFor2.Name = "test author";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync("test");

        searchedAuthors.Count().Should().Be(2);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
        searchedAuthors.Contains(authorToSearchFor2).Should().BeTrue();
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (int i = 0; i < 100; i++)
        {
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        // get random author from the list
        var authorToSearchFor1 = authors[_random.Next(authors.Count)];
        authorToSearchFor1.Name = "test not so";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync("\t test not so          \n ");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
    }

    [Fact]
    public async void SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (int i = 0; i < 100; i++)
        {
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        // get random authors from the list
        var authorToSearchFor1 = authors[_random.Next(authors.Count)];
        var authorToSearchFor2 = authors[_random.Next(authors.Count)];
        authorToSearchFor1.Name = "test not so";
        authorToSearchFor2.Name = "not so test";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync($"{AuthorSearcher.FILTER_OPERATOR}test");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
    }

    [Fact]
    public async void SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (int i = 0; i < 100; i++)
        {
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        // get random authors from the list
        var authorToSearchFor1 = authors[_random.Next(authors.Count)];
        var authorToSearchFor2 = authors[_random.Next(authors.Count)];
        authorToSearchFor1.Name = "not so test";
        authorToSearchFor2.Name = "test not so";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync($"test{AuthorSearcher.FILTER_OPERATOR}");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
    }
}
