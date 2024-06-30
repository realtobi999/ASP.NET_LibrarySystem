using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IBookReviewService
{
    Task<BookReview> Get(Guid id);
    Task<BookReview> Create(CreateBookReviewDto createBookReviewDto);
    Task<int> Delete(Guid id);
    Task<int> Delete(BookReview bookReview);
}