using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services.Books;

namespace LibrarySystem.Application.Services.Books;

internal sealed class BookRecommender : IBookRecommender
{
    private readonly IRepositoryManager _repository;

    public BookRecommender(IRepositoryManager repository)
    {
        _repository = repository;
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
            if (borrow.Book is not null)
            {
                userFavoriteBooks.Add(borrow.Book);
            }
        }

        foreach (var review in user.BookReviews)
        {
            if (review is { Rating: > 5.5, Book: not null })
            {
                userFavoriteBooks.Add(review.Book);
            }
        }

        // extract preferred genres and authors from user favorite books 
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

        var userRecommendedBooks = new HashSet<Book>();

        // recommend books based on matching genres or authors
        foreach (var book in await _repository.Book.IndexAsync())
        {
            if (!userFavoriteBooks.Contains(book) && (book.Genres.Any(g => userPreferredGenreIds.Contains(g.Id)) || book.Authors.Any(a => userPreferredAuthorIds.Contains(a.Id))))
            {
                userRecommendedBooks.Add(book);
            }
        }

        return userRecommendedBooks;
    }
}