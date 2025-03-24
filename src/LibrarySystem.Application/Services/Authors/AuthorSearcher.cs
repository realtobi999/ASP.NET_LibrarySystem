using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;

namespace LibrarySystem.Application.Services.Authors;

public class AuthorSearcher : ISearcher<Author>
{
    public const string FILTER_OPERATOR = Constants.QUERY_SEARCH_FILTER_OPERATOR;

    private readonly IRepositoryManager _repository;

    public AuthorSearcher(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Author>> SearchAsync(string query)
    {
        var authors = await _repository.Author.IndexAsync();

        query = query.Trim();

        if (query.StartsWith(FILTER_OPERATOR))
        {
            return authors.Where(a => a.Name.StartsWith(query[FILTER_OPERATOR.Length..]));
        }

        if (query.EndsWith(FILTER_OPERATOR))
        {
            return authors.Where(a => a.Name.EndsWith(query[..^FILTER_OPERATOR.Length]));
        }

        return authors.Where(a => a.Name.Contains(query));
    }
}