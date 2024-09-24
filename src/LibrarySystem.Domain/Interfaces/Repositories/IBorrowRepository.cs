using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<Borrow>> Index();

    Task<Borrow?> Get(Guid id);
    Task<Borrow?> Get(Guid bookId, Guid userId);
    void Create(Borrow borrow);
}
