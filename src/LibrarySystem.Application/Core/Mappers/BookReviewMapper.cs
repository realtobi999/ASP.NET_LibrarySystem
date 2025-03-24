using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Application.Core.Mappers;

public class BookReviewMapper : IMapper<BookReview, CreateBookReviewDto>
{
    public BookReview Map(CreateBookReviewDto dto)
    {
        return new BookReview
        {
            Id = dto.Id ?? Guid.NewGuid(),
            UserId = dto.UserId,
            BookId = dto.BookId,
            Rating = dto.Rating,
            Text = dto.Text,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}