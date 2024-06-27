using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Interfaces.Services;

public interface IBorrowService
{
    Task<IEnumerable<Borrow>> GetAll();
    Task<Borrow> Get(Guid id);
    Task<Borrow> Get(Guid bookId, Guid userId);
    Task<Borrow> Create(CreateBorrowDto createBorrowDto);
    Task<int> SetIsReturned(Borrow borrow);
    Task<int> SetIsReturned(Borrow borrow, bool returned);
}
