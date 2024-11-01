using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Books;

internal sealed class BookSearcher : ISearcher<Book>
{
    private const string FilterOperator = Constants.QUERY_SEARCH_FILTER_OPERATOR;
    private readonly IRepositoryManager _repository;
    private readonly ISearcher<Genre> _genreSearcher;
    private readonly ISearcher<Author> _authorSearcher;

    public BookSearcher(IRepositoryManager repository, ISearcher<Genre> genreSearcher, ISearcher<Author> authorSearcher)
    {
        _repository = repository;
        _genreSearcher = genreSearcher;
        _authorSearcher = authorSearcher;
    }

    public async Task<IEnumerable<Book>> SearchAsync(string query)
    {
        var books = await _repository.Book.IndexAsync();
        var filteredBooks = new List<Book>();

        query = query.Trim();

        if (query.StartsWith(FilterOperator))
        {
            // search for title or description that starts with the query without the filter operator
            filteredBooks.AddRange(books.Where(b => b.Title!.StartsWith(query[FilterOperator.Length..]) || b.Description!.StartsWith(query[FilterOperator.Length..])));
        }
        else if (query.EndsWith(FilterOperator))
        {
            // search for title or description that ends with the query without the filter operator
            filteredBooks.AddRange(books.Where(b => b.Title!.EndsWith(query[..^FilterOperator.Length]) || b.Description!.StartsWith(query[..^FilterOperator.Length])));
        }
        else
        {
            filteredBooks.AddRange(books.Where(b => b.Title!.Contains(query) || b.Description!.Contains(query)));
        }

        // search for filtered genres and authors
        var filteredGenres = await _genreSearcher.SearchAsync(query);
        var filteredAuthors = await _authorSearcher.SearchAsync(query);

        // filter books by genres
        if (filteredGenres.Any())
        {

            filteredBooks.AddRange(books.Where(b => b.Genres.Any(g => filteredGenres.Select(genre => genre.Id).Contains(g.Id))));
        }

        // filter books by authors
        if (filteredAuthors.Any())
        {
            filteredBooks.AddRange(books.Where(b => b.Authors.Any(a => filteredAuthors.Select(author => author.Id).Contains(a.Id))));
        }

        return filteredBooks.Distinct();
    }
}
