using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;

/// <inheritdoc/>
public interface IBookReviewMapper : IMapper<BookReview, CreateBookReviewDto, UpdateBookReviewDto>
{

}
