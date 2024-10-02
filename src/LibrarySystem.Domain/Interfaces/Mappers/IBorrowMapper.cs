using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;

/// <inheritdoc/>
public interface IBorrowMapper : ICreateMapper<Borrow, CreateBorrowDto>
{
}
