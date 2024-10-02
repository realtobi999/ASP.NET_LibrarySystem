using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Infrastructure.Persistence.Repositories;

public class BorrowRepository : BaseRepository<Borrow>, IBorrowRepository
{
    public BorrowRepository(LibrarySystemContext context) : base(context)
    {
    }

    public async Task<Borrow?> GetAsync(Guid id)
    {
        return await this.GetAsync(b => b.Id == id);
    }

    public async Task<Borrow?> GetAsync(Guid bookId, Guid userId)
    {
        return await this.GetAsync(b => b.BookId == bookId && b.UserId == userId);
    }

    protected override IQueryable<Borrow> GetQueryable()
    {
        return base.GetQueryable().OrderBy(b => b.BorrowDate);
    }
}
