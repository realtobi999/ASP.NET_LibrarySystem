using LibrarySystem.Application.Contracts.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace LibrarySystem.Application.Services.Authors;

public class AuthorService : IAuthorService
{
    private readonly IRepositoryManager _repository;

    public AuthorService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Author> Get(Guid id)
    {
        var author = await _repository.Author.Get(id) ?? throw new AuthorNotFoundException(id);

        return author;
    }

    public async Task<IEnumerable<Author>> GetAll()
    {
        var authors = await _repository.Author.GetAll();

        return authors;
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
        if (!picture.IsNullOrEmpty())
        {
            author.ProfilePicture = Convert.FromBase64String(picture!);    
        }

        _repository.Author.Create(author);
        await _repository.SaveAsync();

        return author;
    }

    public async Task<int> Update(Guid id, UpdateAuthorDto updateAuthorDto)
    {
        var author = await _repository.Author.Get(id) ?? throw new AuthorNotFoundException(id);

        var name = updateAuthorDto.Name;
        var description = updateAuthorDto.Description;
        var birthday = updateAuthorDto.Birthday;
        var picture = updateAuthorDto.ProfilePicture;

        if (!name.IsNullOrEmpty())
        {
            author.Name = name;
        }
        if (!description.IsNullOrEmpty())
        {
            author.Description = description;
        }

        author.Birthday = birthday;

        if (!picture.IsNullOrEmpty())
        {
            author.ProfilePicture = Convert.FromBase64String(picture!);    
        }

        return await _repository.SaveAsync();
    }
}
