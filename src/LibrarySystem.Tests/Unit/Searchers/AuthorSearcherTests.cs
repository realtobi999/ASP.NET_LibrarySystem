using LibrarySystem.Application.Services.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Searchers;

public class AuthorSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly ISearcher<Author> _searcher;
    private static readonly Random Random = new();

    public AuthorSearcherTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _searcher = new AuthorSearcher(_repository.Object);
    }

    [Fact]
    public async Task SearchAsync_ShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (var i = 0; i < 100; i++)
        {
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random authors from the list
        var authorToSearchFor1 = authors[Random.Next(authors.Count)];
        var authorToSearchFor2 = authors[Random.Next(authors.Count)];
        authorToSearchFor1.Name = $"test {keyword}";
        authorToSearchFor2.Name = $"{keyword} test";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync(keyword);

        searchedAuthors.Count().Should().Be(2);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
        searchedAuthors.Contains(authorToSearchFor2).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (var i = 0; i < 100; i++)
        {
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random author from the list
        var authorToSearchFor1 = authors[Random.Next(authors.Count)];
        authorToSearchFor1.Name = $"test {keyword}";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync($"\t {keyword}          \n ");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (var i = 0; i < 100; i++)
        {
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random authors from the list
        var authorToSearchFor1 = authors[Random.Next(authors.Count)];
        var authorToSearchFor2 = authors[Random.Next(authors.Count)];
        authorToSearchFor1.Name = $"{keyword} test";
        authorToSearchFor2.Name = $"test {keyword}";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync($"{AuthorSearcher.FILTER_OPERATOR}{keyword}");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var authors = new List<Author>();

        for (var i = 0; i < 100; i++)
        {
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random authors from the list
        var authorToSearchFor1 = authors[Random.Next(authors.Count)];
        var authorToSearchFor2 = authors[Random.Next(authors.Count)];
        authorToSearchFor1.Name = $"test {keyword}";
        authorToSearchFor2.Name = $"{keyword} test";

        _repository.Setup(r => r.Author.IndexAsync()).ReturnsAsync(authors.OrderBy(a => a.CreatedAt));

        // act & assert
        var searchedAuthors = await _searcher.SearchAsync($"{keyword}{AuthorSearcher.FILTER_OPERATOR}");

        searchedAuthors.Count().Should().Be(1);
        searchedAuthors.Contains(authorToSearchFor1).Should().BeTrue();
    }
}