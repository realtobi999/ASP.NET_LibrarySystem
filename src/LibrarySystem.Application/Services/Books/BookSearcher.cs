using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Books;

internal sealed class BookSearcher : ISearcher<Book>
{
    private const string FilterOperator = "%";
    private readonly IRepositoryManager _repository;

    public BookSearcher(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Book>> SearchAsync(string query)
    {
        var books = await _repository.Book.IndexAsync();
        query = query.Trim();

        if (query.StartsWith(FilterOperator))
        {
            // search for title or description that starts with the query without the filter operator
            return books.Where(b => b.Title!.StartsWith(query[FilterOperator.Length..]) || b.Description!.StartsWith(query[FilterOperator.Length..]));
        }
        else if (query.EndsWith(FilterOperator))
        {
            // search for title or description that ends with the query without the filter operator
            return books.Where(b => b.Title!.EndsWith(query[..^FilterOperator.Length]) || b.Description!.StartsWith(query[..^FilterOperator.Length]));
        }
        else
        {
            return books.Where(b => b.Title!.Contains(query) || b.Description!.Contains(query));
        }
    }
}
