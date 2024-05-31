using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Authors;

public class AuthorService : IAuthorService
{
    private readonly IRepositoryManager _repository;

    public AuthorService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Author> Create(CreateAuthorDto createAuthorDto)
    {
        var picture = createAuthorDto.ProfilePicture;
        var author = new Author
        {
            Id = createAuthorDto.Id ?? Guid.NewGuid(),
            Name = createAuthorDto.Name ?? throw new ArgumentException("The name must be set."),
            Description = createAuthorDto.Description ?? throw new ArgumentException("The description must be set."),
            Birthday = createAuthorDto.Birthday.ToUniversalTime(),
        };

        // if the picture isn't null convert it into byte array and save to the author entity
        if (picture is not null)
        {
            author.ProfilePicture = Convert.FromBase64String(picture);    
        }

        _repository.Author.Create(author);
        await _repository.SaveAsync();

        return author;
    }
}
