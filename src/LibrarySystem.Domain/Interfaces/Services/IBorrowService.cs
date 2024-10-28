using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IBorrowService : IBaseService<Borrow>
{
    Task<Borrow> GetAsync(Guid bookId, Guid userId);
    Task UpdateIsReturnedAsync(Borrow borrow, bool isReturned);
}
