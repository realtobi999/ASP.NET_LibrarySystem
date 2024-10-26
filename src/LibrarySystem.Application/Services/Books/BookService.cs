using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services;

namespace LibrarySystem.Application.Services.Books;

public class BookService : IBookService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Book> _validator;

    public BookService(IRepositoryManager repository, IValidator<Book> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task CreateAsync(Book book)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(book);
        if (!valid && exception is not null) throw exception;

        // create book and save changes
        _repository.Book.Create(book);
        await _repository.SaveSafelyAsync();
    }

    public async Task DeleteAsync(Book book)
    {
        // delete book and save changes
        _repository.Book.Delete(book);
        await _repository.SaveSafelyAsync();
    }

    public async Task<Book> GetAsync(Guid id)
    {
        var book = await _repository.Book.GetAsync(id) ?? throw new NotFound404Exception(nameof(Book), id);

        return book;
    }

    public async Task<IEnumerable<Book>> IndexAsync()
    {
        var books = await _repository.Book.IndexAsync();

        return books;
    }

    public async Task<Book> GetAsync(string isbn)
    {
        var book = await _repository.Book.GetAsync(isbn) ?? throw new NotFound404Exception(nameof(Book), $"ISBN {isbn}");

        return book;
    }

    public async Task UpdateAsync(Book book)
    {
        // validate
        var (valid, exception) = await _validator.ValidateAsync(book);
        if (!valid && exception is not null) throw exception;

        // update book and save changes
        _repository.Book.Update(book);
        await _repository.SaveSafelyAsync();
    }

    public async Task UpdateAvailabilityAsync(Book book, bool isAvailable)
    {
        book.UpdateAvailability(isAvailable);

        await this.UpdateAsync(book);
    }

    public async Task UpdatePopularityAsync(Book book, double popularity)
    {
        book.UpdatePopularity(popularity);
        await this.UpdateAsync(book);
    }

    public async Task<IEnumerable<Book>> IndexRecommendedAsync(User user)
    {
        // use HashSet to avoid duplicate values
        var userFavoriteBooks = new HashSet<Book>();
        var userPreferredGenreIds = new HashSet<Guid>();
        var userPreferredAuthorIds = new HashSet<Guid>();

        // collect user favorite books from wishlists, borrows, and reviews
        foreach (var wishlist in user.Wishlists)
        {
            foreach (var book in wishlist.Books)
            {
                userFavoriteBooks.Add(book);
            }
        }

        foreach (var borrow in user.Borrows)
        {
            userFavoriteBooks.Add(borrow.Book!);
        }

        foreach (var review in user.BookReviews)
        {
            if (review.Rating > 5.5)
            {
                userFavoriteBooks.Add(review.Book!);
            }
        }

        // extract preferred genres and authors from userFavoriteBooks 
        foreach (var book in userFavoriteBooks)
        {
            foreach (var genre in book.Genres)
            {
                userPreferredGenreIds.Add(genre.Id);
            }
            foreach (var author in book.Authors)
            {
                userPreferredAuthorIds.Add(author.Id);
            }
        }

        var books = await this.IndexAsync();
        var userRecommendedBooks = new HashSet<Book>();

        // recommend books based on matching genres or authors
        foreach (var book in books)
        {
            if (!userFavoriteBooks.Contains(book) && (book.Genres.Any(g => userPreferredGenreIds.Contains(g.Id)) || book.Authors.Any(a => userPreferredAuthorIds.Contains(a.Id))))
            {
                userRecommendedBooks.Add(book);
            }
        }

        // return sorted recommended books by popularity
        return userRecommendedBooks.OrderBy(b => b.Popularity);
    }
}
