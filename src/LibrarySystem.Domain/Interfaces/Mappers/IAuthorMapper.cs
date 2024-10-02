using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Mappers;

/// <inheritdoc/>
public interface IAuthorMapper : IMapper<Author, CreateAuthorDto, UpdateAuthorDto>
{

}
