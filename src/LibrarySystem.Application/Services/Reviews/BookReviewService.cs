using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Reviews;

public class BookReviewService : IBookReviewService
{
    private readonly IRepositoryManager _repository;

    public BookReviewService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<BookReview> Create(CreateBookReviewDto createBookReviewDto)
    {
        var user = await _repository.User.Get(createBookReviewDto.UserId) ?? throw new NotFound404Exception(nameof(User), createBookReviewDto.UserId);
        var book = await _repository.Book.Get(createBookReviewDto.BookId) ?? throw new NotFound404Exception(nameof(Book), createBookReviewDto.BookId);

        var review = new BookReview
        {
            Id = createBookReviewDto.Id ?? Guid.NewGuid(),
            UserId = user.Id,
            BookId = book.Id,
            Rating = createBookReviewDto.Rating,
            Text = createBookReviewDto.Text,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        _repository.BookReview.Create(review);
        await _repository.SaveAsync();

        return review;
    }

    public async Task<int> Delete(Guid id)
    {
        var review = await this.Get(id);

        _repository.BookReview.Delete(review);
        return await _repository.SaveAsync();
    }

    public async Task<int> Delete(BookReview bookReview)
    {
        _repository.BookReview.Delete(bookReview);
        return await _repository.SaveAsync();

    }

    public async Task<BookReview> Get(Guid id)
    {
        var review = await _repository.BookReview.Get(id) ?? throw new NotFound404Exception(nameof(BookReview), id);

        return review;
    }

    public async Task<int> Update(Guid id, UpdateBookReviewDto updateBookReviewDto)
    {
        var review = await this.Get(id);

        return await this.Update(review, updateBookReviewDto);
    }

    public async Task<int> Update(BookReview bookReview, UpdateBookReviewDto updateBookReviewDto)
    {
        var text = updateBookReviewDto.Text;
        var rating = updateBookReviewDto.Rating;

        bookReview.Text = text;
        bookReview.Rating = rating;

        return await _repository.SaveAsync();
    }
}
