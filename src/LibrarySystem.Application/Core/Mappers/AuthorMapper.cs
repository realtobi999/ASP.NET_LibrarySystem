using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Application.Core.Mappers;

public class AuthorMapper : IAuthorMapper
{
    public Author CreateFromDto(CreateAuthorDto dto)
    {
        return new Author
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Birthday = dto.Birthday.ToUniversalTime()
        };
    }

    public void UpdateFromDto(Author author, UpdateAuthorDto dto)
    {
        author.Name = dto.Name;
        author.Description = dto.Description;
        author.Birthday = dto.Birthday;
    }
}
