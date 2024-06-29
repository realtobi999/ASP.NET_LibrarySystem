using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class BookReviewRepository : IBookReviewRepository
{
    private readonly LibrarySystemContext _context;

    public BookReviewRepository(LibrarySystemContext context)
    {
        _context = context;
    }

    public void Create(BookReview bookReview)
    {
        _context.BookReviews.Add(bookReview);
    }
}
