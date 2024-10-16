using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Reviews;

public class BookReviewService : IBookReviewService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<BookReview> _validator;

    public BookReviewService(IRepositoryManager repository, IValidator<BookReview> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task CreateAsync(BookReview review)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(review);
        if (!valid && exception is not null) throw exception;

        // create review entity and save changes
        _repository.BookReview.Create(review);
        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(BookReview review)
    {
        // delete review entity and save changes
        _repository.BookReview.Delete(review);
        await _repository.SaveSafelyAsync();
    }

    public async Task<BookReview> GetAsync(Guid id)
    {
        var review = await _repository.BookReview.GetAsync(id) ?? throw new NotFound404Exception(nameof(BookReview), id);

        return review;
    }

    public async Task<IEnumerable<BookReview>> IndexAsync()
    {
        var reviews = await _repository.BookReview.IndexAsync();

        return reviews;
    }

    public async Task UpdateAsync(BookReview review)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(review);
        if (!valid && exception is not null) throw exception;

        // update review entity and save changes
        _repository.BookReview.Update(review);
        await _repository.SaveSafelyAsync();
    }
}
