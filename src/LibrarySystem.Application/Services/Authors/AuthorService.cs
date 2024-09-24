﻿using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

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
        var author = await _repository.Author.Get(id) ?? throw new NotFound404Exception(nameof(Author), id);

        return author;
    }

    public async Task<IEnumerable<Author>> Index()
    {
        var authors = await _repository.Author.Index();

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

        _repository.Author.Create(author);
        await _repository.SaveAsync();

        return author;
    }

    public async Task<int> Update(Guid id, UpdateAuthorDto updateAuthorDto)
    {
        var author = await this.Get(id);

        var name = updateAuthorDto.Name;
        var description = updateAuthorDto.Description;
        var birthday = updateAuthorDto.Birthday;

        author.Name = name;
        author.Description = description;
        author.Birthday = birthday;

        return await _repository.SaveAsync();
    }

    public async Task<int> Delete(Guid id)
    {
        var author = await this.Get(id);

        _repository.Author.Delete(author);
        return await _repository.SaveAsync();
    }
}
