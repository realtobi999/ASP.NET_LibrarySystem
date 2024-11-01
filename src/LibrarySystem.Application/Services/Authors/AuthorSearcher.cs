using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Authors;

public class AuthorSearcher : ISearcher<Author>
{
    private const string FilterOperator = Constants.QUERY_SEARCH_FILTER_OPERATOR;
    private readonly IRepositoryManager _repository;

    public AuthorSearcher(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Author>> SearchAsync(string query)
    {
        var authors = await _repository.Author.IndexAsync();

        query = query.Trim();

        if (query.StartsWith(FilterOperator))
        {
            return authors.Where(a => a.Name!.StartsWith(query[FilterOperator.Length..]));
        }
        else if (query.EndsWith(FilterOperator))
        {
            return authors.Where(a => a.Name!.EndsWith(query[..^FilterOperator.Length]));
        }
        else
        {
            return authors.Where(a => a.Name!.Contains(query));
        }
    }
}
