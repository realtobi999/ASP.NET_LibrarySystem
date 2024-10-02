using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;

/// <inheritdoc/>
public interface IBookMapper : IMapper<Book, CreateBookDto, UpdateBookDto>
{

}
