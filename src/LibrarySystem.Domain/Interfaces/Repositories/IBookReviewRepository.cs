using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBookReviewRepository
{
    void Create(BookReview bookReview);
}
