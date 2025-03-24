using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services.Books;

namespace LibrarySystem.Application.Services.Books;

public sealed class BookService : IBookService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Book> _validator;
    private readonly ISearcher<Book> _searcher;
    private readonly IBookRecommender _recommender;
    private readonly IBookPopularityCalculator _calculator;

    public BookService(IRepositoryManager repository, IValidator<Book> validator, IBookRecommender recommender, ISearcher<Book> searcher, IBookPopularityCalculator calculator)
    {
        _repository = repository;
        _validator = validator;
        _recommender = recommender;
        _searcher = searcher;
        _calculator = calculator;
    }

    public async Task CreateAsync(Book book)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(book);
        if (!valid && exception is not null) throw exception;

        // create book and save changes
        _repository.Book.Create(book);
        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Book book)
    {
        // delete book and save changes
        _repository.Book.Delete(book);
        await _repository.SaveSafelyAsync();
    }

    public async Task<Book> GetAsync(Guid id)
    {
        var book = await _repository.Book.GetAsync(id) ?? throw new NotFound404Exception(nameof(Book), id);

        return book;
    }

    public async Task<IEnumerable<Book>> IndexAsync()
    {
        var books = await _repository.Book.IndexAsync();

        return books;
    }

    public async Task<Book> GetAsync(string isbn)
    {
        var book = await _repository.Book.GetAsync(isbn) ?? throw new NotFound404Exception(nameof(Book), $"ISBN {isbn}");

        return book;
    }

    public async Task UpdateAsync(Book book)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(book);
        if (!valid && exception is not null) throw exception;

        // update book and save changes
        _repository.Book.Update(book);
        await _repository.SaveSafelyAsync();
    }

    public async Task UpdateAvailabilityAsync(Book book, bool isAvailable)
    {
        book.UpdateIsAvailable(isAvailable);

        await this.UpdateAsync(book);
    }

    public async Task UpdatePopularityAsync(Book book, double popularity)
    {
        book.UpdatePopularity(popularity);

        await this.UpdateAsync(book);
    }

    public async Task<IEnumerable<Book>> IndexRecommendedAsync(User user)
    {
        return (await _recommender.IndexRecommendedAsync(user)).OrderBy(b => b.Popularity);
    }

    public async Task<IEnumerable<Book>> SearchAsync(string query)
    {
        return await _searcher.SearchAsync(query);
    }

    public double CalculateBookPopularity(Book book)
    {
        return _calculator.CalculatePopularityScore(book);
    }
}