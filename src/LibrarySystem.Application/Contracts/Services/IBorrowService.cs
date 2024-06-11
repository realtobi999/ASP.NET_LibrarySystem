using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IBorrowService
{
    Task<Borrow> Create(CreateBorrowDto createBorrowDto);
}
