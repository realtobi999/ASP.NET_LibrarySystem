using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;

namespace LibrarySystem.Application.Services.Genres;

public class GenreSearcher : ISearcher<Genre>
{
    public const string FILTER_OPERATOR = Constants.QUERY_SEARCH_FILTER_OPERATOR;

    private readonly IRepositoryManager _repository;

    public GenreSearcher(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Genre>> SearchAsync(string query)
    {
        var genres = await _repository.Genre.IndexAsync();

        query = query.Trim();

        if (query.StartsWith(FILTER_OPERATOR))
        {
            return genres.Where(a => a.Name.StartsWith(query[FILTER_OPERATOR.Length..]));
        }

        if (query.EndsWith(FILTER_OPERATOR))
        {
            return genres.Where(a => a.Name.EndsWith(query[..^FILTER_OPERATOR.Length]));
        }

        return genres.Where(a => a.Name.Contains(query));
    }
}