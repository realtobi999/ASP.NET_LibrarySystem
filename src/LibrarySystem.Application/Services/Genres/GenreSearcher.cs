using System.Security.AccessControl;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Genres;

public class GenreSearcher : ISearcher<Genre>
{
    private const string FilterOperator = Constants.QUERY_SEARCH_FILTER_OPERATOR;
    private readonly IRepositoryManager _repository;

    public GenreSearcher(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Genre>> SearchAsync(string query)
    {
        var genres = await _repository.Genre.IndexAsync();

        query = query.Trim();

        if (query.StartsWith(FilterOperator))
        {
            return genres.Where(a => a.Name!.StartsWith(query[FilterOperator.Length..]));
        }
        else if (query.EndsWith(FilterOperator))
        {
            return genres.Where(a => a.Name!.EndsWith(query[..^FilterOperator.Length]));
        }
        else
        {
            return genres.Where(a => a.Name!.Contains(query));
        }
    }
}
