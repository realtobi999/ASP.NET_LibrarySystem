using System.Collections;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Books;

public class BookService : IBookService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Book> _validator;
    private readonly IBookRecommender _recommender;

    public BookService(IRepositoryManager repository, IValidator<Book> validator, IBookRecommender recommender)
    {
        _repository = repository;
        _validator = validator;
        _recommender = recommender;
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
        book.UpdateAvailability(isAvailable);

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
        var books = await this.IndexAsync();

        return books.Where(b => b.Title!.Contains(query!) || b.Description!.Contains(query!));
    }
}
