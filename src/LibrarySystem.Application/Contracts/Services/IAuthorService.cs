using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IAuthorService
{
    Task<Author> Create(CreateAuthorDto createAuthorDto);
    Task<Author> Get(Guid id);
    Task<IEnumerable<Author>> GetAll();
}
