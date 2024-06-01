using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IAuthorService
{
    Task<Author> Get(Guid id);
    Task<IEnumerable<Author>> GetAll();
    Task<Author> Create(CreateAuthorDto createAuthorDto);
    Task<int> Update(Guid id, UpdateAuthorDto updateAuthorDto);
    Task<int> Delete(Guid id);
}
