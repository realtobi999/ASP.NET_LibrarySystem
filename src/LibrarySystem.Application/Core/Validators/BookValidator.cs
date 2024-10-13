using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;

namespace LibrarySystem.Application.Core.Validators;

public class BookValidator : IValidator<Book>
{
    private readonly IRepositoryManager _repository;

    public BookValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> ValidateAsync(Book book)
    {
        // validate that the book has at least one genre and author assigned
        if (book.BookAuthors.Count < 1 || book.BookGenres.Count < 1)
        {
            return (false, new BadRequest400Exception("A book must have at least one author and one genre assigned."));
        }

        // validate that the assigned genres exists
        foreach (var genreId in book.BookGenres.Select(bk => bk.GenreId))
        {
            var genre = await _repository.Genre.GetAsync(genreId);

            if (genre is null)
            {
                return (false, new NotFound404Exception(nameof(Genre), genreId));
            }
        }

        // validate that the assigned authors exists
        foreach (var authorId in book.BookAuthors.Select(ba => ba.AuthorId))
        {
            var author = await _repository.Author.GetAsync(authorId);

            if (author is null)
            {
                return (false, new NotFound404Exception(nameof(Author), authorId));
            }
        }

        // validate that the assigned book reviews have the correct book id
        foreach (var review in book.BookReviews)
        {
            if (review.BookId != book.Id)
            {
                return (false, new Conflict409Exception($"Review with ID {review.Id} is incorrectly associated with Book ID {review.BookId} instead of Book ID {book.Id}."));
            }
        }

        return (true, null);
    }
}
