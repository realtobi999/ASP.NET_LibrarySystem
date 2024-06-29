using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
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
        var user = await _repository.User.Get(createBookReviewDto.UserId) ?? throw new UserNotFoundException(createBookReviewDto.UserId);
        var book = await _repository.Book.Get(createBookReviewDto.BookId) ?? throw new BookNotFoundException(createBookReviewDto.BookId);

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
}
