using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookReviewRepository
{
    Task<BookReview?> Get(Guid id);
    void Create(BookReview bookReview);
    void Delete(BookReview bookReview);
}
