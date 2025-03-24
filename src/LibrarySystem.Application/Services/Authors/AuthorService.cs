using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Authors;

public class AuthorService : IAuthorService
{
    private readonly IRepositoryManager _repository;

    public AuthorService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Author> GetAsync(Guid id)
    {
        var author = await _repository.Author.GetAsync(id) ?? throw new NotFound404Exception(nameof(Author), id);

        return author;
    }

    public async Task<IEnumerable<Author>> IndexAsync()
    {
        var authors = await _repository.Author.IndexAsync();

        return authors;
    }

    public async Task CreateAsync(Author author)
    {
        _repository.Author.Create(author);

        await _repository.SaveSafelyAsync();
    }

    public async Task UpdateAsync(Author author)
    {
        _repository.Author.Update(author);

        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Author author)
    {
        _repository.Author.Delete(author);

        await _repository.SaveSafelyAsync();
    }
}