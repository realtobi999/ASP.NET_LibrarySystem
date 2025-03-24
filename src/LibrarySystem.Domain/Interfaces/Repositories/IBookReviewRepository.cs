using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookReviewRepository : IBaseRepository<BookReview>
{
    Task<BookReview?> GetAsync(Guid id);
}