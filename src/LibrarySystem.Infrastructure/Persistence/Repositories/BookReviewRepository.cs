using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public void Delete(BookReview bookReview)
    {
        _context.BookReviews.Remove(bookReview);
    }

    public async Task<BookReview?> Get(Guid id)
    {
        return await _context.BookReviews.FirstOrDefaultAsync(br => br.Id == id);
    }
}
