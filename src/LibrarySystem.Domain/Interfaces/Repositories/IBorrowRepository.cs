using System.Reflection;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<Borrow>> GetAll();

    Task<Borrow?> Get(Guid id);
    void Create(Borrow borrow);
}
