using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Domain.Interfaces.Services.Books;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Books;

public class BookRecommenderTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly IBookRecommender _recommender;
    private static readonly Random Random = new();

    public BookRecommenderTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _recommender = new BookRecommender(_repository.Object);
    }

    [Fact]
    public async Task IndexRecommendedAsync_ReturnsCorrectlyRecommendedBooks()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();

        var books = new List<Book>();
        var genres = new List<Genre>();
        var authors = new List<Author>();

        for (var i = 0; i < 5; i++)
        {
            genres.Add(GenreFactory.CreateWithFakeData());
            authors.Add(AuthorFactory.CreateWithFakeData());
        }

        for (var i = 0; i < 100; i++)
        {
            var book = BookFactory.CreateWithFakeData();

            book.Genres.Add(genres[Random.Next(genres.Count)]);
            book.Authors.Add(authors[Random.Next(authors.Count)]);

            books.Add(book);
        }

        // assign books to user review and borrow
        var userPreferredBook1 = books[Random.Next(books.Count)];

        user.Borrows.Add(BorrowFactory.CreateWithFakeData(userPreferredBook1, user));
        user.BookReviews.Add(BookReviewFactory.CreateWithFakeData(userPreferredBook1, user));

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