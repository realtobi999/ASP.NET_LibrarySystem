using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Helpers;

internal static class AuthorTestExtensions
{
    public static CreateAuthorDto ToCreateAuthorDto(this Author author)
    {
        return new CreateAuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Description = author.Description,
            Birthday = author.Birthday
        };
    }
}