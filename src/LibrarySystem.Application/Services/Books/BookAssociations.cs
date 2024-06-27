﻿using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Services.Books;

public class BookAssociations : IBookAssociations
{
    private readonly IRepositoryManager _repository;

    public BookAssociations(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task AssignAuthorsAsync(IEnumerable<Guid> authorIds, Book book)
    {
        if (authorIds == null) throw new ArgumentNullException(nameof(authorIds));

        var tasks = authorIds.Select(async authorId =>
        {
            var author = await _repository.Author.Get(authorId) ?? throw new AuthorNotFoundException(authorId);
            _repository.Associations.CreateBookAuthor(new BookAuthor
            {
                BookId = book.Id,
                Book = book,
                AuthorId = author.Id,
                Author = author
            });
        });

        await Task.WhenAll(tasks);
    }

    public async Task AssignGenresAsync(IEnumerable<Guid> genreIds, Book book)
    {
        if (genreIds == null) throw new ArgumentNullException(nameof(genreIds));

        var tasks = genreIds.Select(async genreId =>
        {
            var genre = await _repository.Genre.Get(genreId) ?? throw new GenreNotFoundException(genreId);
            _repository.Associations.CreateBookGenre(new BookGenre
            {
                BookId = book.Id,
                Book = book,
                GenreId = genre.Id,
                Genre = genre,
            });
        });

        await Task.WhenAll(tasks);
    }

    public void CleanAuthors(Book book)
    {
        foreach (var association in book.BookAuthors)
        {
            _repository.Associations.RemoveBookAuthor(association);
        }
    }

    public void CleanGenres(Book book)
    {
        foreach (var association in book.BookGenres)
        {
            _repository.Associations.RemoveBookGenre(association);
        }
    }
}
