using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Application.Contracts.Services;

public interface IAuthorService
{
    Task<Author> Create(CreateAuthorDto createAuthorDto);
}
