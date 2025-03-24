using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Application.Core.Mappers;

public class AuthorMapper : IMapper<Author, CreateAuthorDto>
{
    public Author Map(CreateAuthorDto dto)
    {
        return new Author
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Birthday = dto.Birthday.ToUniversalTime(),
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}