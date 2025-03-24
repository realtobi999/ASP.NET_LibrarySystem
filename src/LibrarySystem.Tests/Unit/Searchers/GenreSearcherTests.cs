using LibrarySystem.Application.Services.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Searchers;

public class GenreSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly ISearcher<Genre> _searcher;
    private static readonly Random Random = new();

    public GenreSearcherTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _searcher = new GenreSearcher(_repository.Object);
    }

    [Fact]
    public async Task SearchAsync_ShouldWorkWithBasicQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (var i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random genre from the list
        var genreToSearchFor = genres[Random.Next(genres.Count)];
        genreToSearchFor.Name = $"test {keyword}";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres);

        // act & assert
        var searchedGenres = await _searcher.SearchAsync(keyword);

        searchedGenres.Count().Should().Be(1);
        searchedGenres.Contains(genreToSearchFor).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (var i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random genre from the list
        var genreToSearchFor = genres[Random.Next(genres.Count)];
        genreToSearchFor.Name = $"test {keyword}";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres.OrderBy(g => g.CreatedAt));

        // act & assert
        var searchedGenres = await _searcher.SearchAsync($"\t {keyword}          \n ");

        searchedGenres.Count().Should().Be(1);
        searchedGenres.Contains(genreToSearchFor).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (var i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random genres from the list
        var genreToSearchFor1 = genres[Random.Next(genres.Count)];
        var genreToSearchFor2 = genres[Random.Next(genres.Count)];
        genreToSearchFor1.Name = $"{keyword} test";
        genreToSearchFor2.Name = $"test {keyword}";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres.OrderBy(g => g.CreatedAt));

        // act & assert
        var searchedGenres = await _searcher.SearchAsync($"{GenreSearcher.FILTER_OPERATOR}{keyword}");

        searchedGenres.Count().Should().Be(1);
        searchedGenres.Contains(genreToSearchFor1).Should().BeTrue();
    }

    [Fact]
    public async Task SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (var i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var keyword = Guid.NewGuid().ToString();

        // get random genres from the list
        var genreToSearchFor1 = genres[Random.Next(genres.Count)];
        var genreToSearchFor2 = genres[Random.Next(genres.Count)];
        genreToSearchFor1.Name = $"test {keyword}";
        genreToSearchFor2.Name = $"{keyword} test";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres.OrderBy(g => g.CreatedAt));

        // act & assert
        var searchedGenres = await _searcher.SearchAsync($"{keyword}{GenreSearcher.FILTER_OPERATOR}");

        searchedGenres.Count().Should().Be(1);
        searchedGenres.Contains(genreToSearchFor1).Should().BeTrue();
    }
}