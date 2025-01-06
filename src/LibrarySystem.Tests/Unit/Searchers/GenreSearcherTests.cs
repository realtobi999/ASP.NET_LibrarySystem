using LibrarySystem.Application.Services.Genres;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using Moq;

namespace LibrarySystem.Tests.Unit.Searchers;

public class GenreSearcherTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly ISearcher<Genre> _searcher;
    private static readonly Random _random = new Random();

    public GenreSearcherTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _searcher = new GenreSearcher(_repository.Object);
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBasicQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (int i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var genreToSearchFor1 = genres[_random.Next(genres.Count)];
        genreToSearchFor1.Name = "test";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres);

        // act & assert
        var searchedGenres = await _searcher.SearchAsync("tes");

        searchedGenres.Count().Should().Be(1);
        searchedGenres.ElementAt(0).Id.Should().Be(genreToSearchFor1.Id);
    }

    [Fact]
    public async void SearchAsync_ShouldWorkWithBadQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (int i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var genreToSearchFor1 = genres[_random.Next(genres.Count)];
        genreToSearchFor1.Name = "test";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres.OrderBy(g => g.CreatedAt));

        // act & assert
        var searchedGenres = await _searcher.SearchAsync("\t test          \n ");

        searchedGenres.Count().Should().Be(1);
        searchedGenres.ElementAt(0).Id.Should().Be(genreToSearchFor1.Id);
    }

    [Fact]
    public async void SearchAsync_StartsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (int i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var genreToSearchFor1 = genres[_random.Next(genres.Count - 1)];
        genreToSearchFor1.Name = "test not so";
        var genreToSearchFor2 = genres[_random.Next(genres.Count + 1)];
        genreToSearchFor2.Name = "so not test";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres.OrderBy(g => g.CreatedAt));

        // act & assert
        var searchedGenres = await _searcher.SearchAsync($"{Constants.QUERY_SEARCH_FILTER_OPERATOR}test");

        searchedGenres.Count().Should().Be(1);
        searchedGenres.ElementAt(0).Id.Should().Be(genreToSearchFor1.Id);
    }

    [Fact]
    public async void SearchAsync_EndsWithFilterShouldWorkWithBasicQueryInput()
    {
        // prepare
        var genres = new List<Genre>();

        for (int i = 0; i < 100; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
        }

        var genreToSearchFor1 = genres[_random.Next(genres.Count - 1)];
        genreToSearchFor1.Name = "so not test";
        var genreToSearchFor2 = genres[_random.Next(genres.Count + 1)];
        genreToSearchFor2.Name = "test not so";

        _repository.Setup(r => r.Genre.IndexAsync()).ReturnsAsync(genres.OrderBy(g => g.CreatedAt));

        // act & assert
        var searchedGenres = await _searcher.SearchAsync($"test{Constants.QUERY_SEARCH_FILTER_OPERATOR}");

        searchedGenres.Count().Should().Be(1);
        searchedGenres.ElementAt(0).Id.Should().Be(genreToSearchFor1.Id);
    }
}
