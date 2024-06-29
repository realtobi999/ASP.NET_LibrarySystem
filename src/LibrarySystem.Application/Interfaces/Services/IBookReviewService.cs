using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IBookReviewService
{
    Task<BookReview> Create(CreateBookReviewDto createBookReviewDto);
}