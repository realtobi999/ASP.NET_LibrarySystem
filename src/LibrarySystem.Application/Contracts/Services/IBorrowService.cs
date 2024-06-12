using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IBorrowService
{
    Task<IEnumerable<Borrow>> GetAll();
    Task<Borrow> Get(Guid id);
    Task<Borrow> Create(CreateBorrowDto createBorrowDto);
}
