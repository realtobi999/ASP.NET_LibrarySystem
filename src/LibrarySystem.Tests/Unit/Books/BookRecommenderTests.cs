using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Domain.Interfaces.Services.Books;
using LibrarySystem.Tests.Integration.Helpers;
using Moq;

namespace LibrarySystem.Tests.Unit.Books;

public class BookRecommenderTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly IBookRecommender _recommender;
    private static readonly Random _random = new Random();

    public BookRecommenderTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _recommender = new BookRecommender(_repository.Object);
    }

    [Fact]
    public async void IndexRecommendedAsync_ReturnsCorrectlyRecommendedBooks()
    {
        // prepare
        var user = new User().WithFakeData();

        var books = new List<Book>();
        var genres = new List<Genre>();
        var authors = new List<Author>();

        for (int i = 0; i < 5; i++)
        {
            genres.Add(new Genre().WithFakeData());
            authors.Add(new Author().WithFakeData());
        }

        for (int i = 0; i < 100; i++)
        {
            var book = new Book().WithFakeData();

            book.Genres.Add(genres[_random.Next(genres.Count)]);
            book.Authors.Add(authors[_random.Next(authors.Count)]);

            books.Add(book);
        }

        // assign books to user review and borrow
        var userPreferredBook1 = books[_random.Next(books.Count)];

        user.Borrows.Add(new Borrow().WithFakeData(userPreferredBook1, user));
        user.BookReviews.Add(new BookReview().WithFakeData(userPreferredBook1, user));

        _repository.Setup(r => r.Book.IndexAsync()).ReturnsAsync(books);

        // act & assert
        var userRecommendedBooks = await _recommender.IndexRecommendedAsync(user);

        foreach (var userRecommendedBook in userRecommendedBooks)
        {
            userRecommendedBook.Genres.Count.Should().Be(1);
            userRecommendedBook.Genres.ElementAt(0).Id.Should().Be(userPreferredBook1.Genres.ElementAt(0).Id);

            userRecommendedBook.Authors.Count.Should().Be(1);
            userRecommendedBook.Authors.ElementAt(0).Id.Should().Be(userPreferredBook1.Genres.ElementAt(0).Id);
        }
    }
}
