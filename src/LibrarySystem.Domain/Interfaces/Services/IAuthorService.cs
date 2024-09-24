using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Services;

public interface IAuthorService
{
    Task<Author> Get(Guid id);
    Task<IEnumerable<Author>> Index();
    Task<Author> Create(CreateAuthorDto createAuthorDto);
    Task<int> Update(Guid id, UpdateAuthorDto updateAuthorDto);
    Task<int> Delete(Guid id);
}
