using LibrarySystem.Application.Interfaces.Services;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.NotFound;
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
        var author = new Author
        {
            Id = createAuthorDto.Id ?? Guid.NewGuid(),
            Name = createAuthorDto.Name ?? throw new NullReferenceException("The name must be set."),
            Description = createAuthorDto.Description ?? throw new NullReferenceException("The description must be set."),
            Birthday = createAuthorDto.Birthday.ToUniversalTime(),
        };
        
        var picture = createAuthorDto.ProfilePicture;

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

        author.Name = name;
        author.Description = description;
        author.Birthday = birthday;

        if (!picture.IsNullOrEmpty())
        {
            author.ProfilePicture = Convert.FromBase64String(picture!);    
        }

        return await _repository.SaveAsync();
    }

    public async Task<int> Delete(Guid id)
    {
        var author = await _repository.Author.Get(id) ?? throw new AuthorNotFoundException(id);

        _repository.Author.Delete(author);
        return await _repository.SaveAsync();
    }
}
