using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class BookReviewMapper : IBookReviewMapper
{
    public BookReview CreateFromDto(CreateBookReviewDto dto)
    {
        return new BookReview
        {
            Id = dto.Id ?? Guid.NewGuid(),
            UserId = dto.UserId,
            BookId = dto.BookId,
            Rating = dto.Rating,
            Text = dto.Text,
            CreatedAt = DateTimeOffset.UtcNow,
        };
    }

    public void UpdateFromDto(BookReview review, UpdateBookReviewDto dto)
    {
        review.Text = dto.Text;
        review.Rating = dto.Rating;
    }
}
