using System.Reflection;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IBorrowRepository
{
    Task<IEnumerable<Borrow>> GetAll();
    void Create(Borrow borrow);
}
