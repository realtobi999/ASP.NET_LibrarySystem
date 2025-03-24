using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBorrowRepository : IBaseRepository<Borrow>
{
    Task<Borrow?> GetAsync(Guid id);
    Task<Borrow?> GetAsync(Guid bookId, Guid userId);
}