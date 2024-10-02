using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class BookReviewRepository : BaseRepository<BookReview>, IBookReviewRepository
{
    public BookReviewRepository(LibrarySystemContext context) : base(context)
    {
    }

    public async Task<BookReview?> GetAsync(Guid id)
    {
        return await this.GetAsync(br => br.Id == id);
    }
}
