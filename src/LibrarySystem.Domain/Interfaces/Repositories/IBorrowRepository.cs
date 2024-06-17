using System.Reflection;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<Borrow>> GetAll();

    Task<Borrow?> Get(Guid id);
    Task<Borrow?> Get(Guid bookId, Guid userId);
    void Create(Borrow borrow);
}
