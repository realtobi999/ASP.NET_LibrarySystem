using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Services.Books;
using LibrarySystem.Tests.Integration.Factories;

namespace LibrarySystem.Tests.Unit.Books;

public class BookCalculatorTests
{
    private readonly IBookPopularityCalculator _calculator;

    public BookCalculatorTests()
    {
        _calculator = new BookPopularityCalculator();
    }

    [Fact]
    public void CalculatePopularityScore_ReturnsCorrectValueBasedOnBorrows()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();

        // assign recent borrows to book1
        for (int i = 0; i < 3; i++)
        {
            book1.Borrows.Add(BorrowFactory.CreateWithFakeData(book1, user));
        }

        // assign not recent borrows to book2
        for (int i = 0; i < 3; i++)
        {
            var borrow = BorrowFactory.CreateWithFakeData(book2, user);
            borrow.BorrowDate = borrow.BorrowDate.AddDays(-(BookPopularityCalculator.RECENT_ACTIVITY_DAYS + 1));

            book2.Borrows.Add(borrow);
        }

        // act & assert
        var book1Popularity = _calculator.CalculatePopularityScore(book1);
        var book2Popularity = _calculator.CalculatePopularityScore(book2);

        book1Popularity.Should().NotBe(0);
        book2Popularity.Should().NotBe(0);
        book1Popularity.Should().NotBe(book2Popularity);
        book1Popularity.Should().BeGreaterThan(book2Popularity);
    }

    [Fact]
    public void CalculateBookPopularity_ReturnsCorrectValueBasedOnPositiveReviews()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();

        // assign recent positive reviews to book1
        for (int i = 0; i < 3; i++)
        {
            var review = BookReviewFactory.CreateWithFakeData(book1, user);
            review.Rating = 10;

            book1.BookReviews.Add(review);
        }

        // assign recent negative reviews to book1
        for (int i = 0; i < 3; i++)
        {
            var review = BookReviewFactory.CreateWithFakeData(book2, user);
            review.Rating = 0;

            book2.BookReviews.Add(review);
        }

        // act & assert
        var book1Popularity = _calculator.CalculatePopularityScore(book1);
        var book2Popularity = _calculator.CalculatePopularityScore(book2);

        book1Popularity.Should().NotBe(0);
        book2Popularity.Should().NotBe(0);
        book1Popularity.Should().NotBe(book2Popularity);
        book1Popularity.Should().BeGreaterThan(book2Popularity);
    }

    [Fact]
    public void CalculateBookPopularity_ReturnsCorrectValueWhenBookIsNotBorrowedNorReviewed()
    {
        // prepare
        var book = BookFactory.CreateWithFakeData();

        // act & assert
        var bookPopularity = _calculator.CalculatePopularityScore(book);

        bookPopularity.Should().Be(book.Popularity - book.Popularity * BookPopularityCalculator.INACTIVITY_PENALTY_MULTIPLIER);
    }
}
