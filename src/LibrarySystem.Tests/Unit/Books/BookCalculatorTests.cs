using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Interfaces.Services.Books;
using LibrarySystem.Tests.Integration.Factories;

namespace LibrarySystem.Tests.Unit.Books;

public class BookCalculatorTests
{
    private readonly IBookCalculator _calculator;

    public BookCalculatorTests()
    {
        _calculator = new BookCalculator();
    }

    [Fact]
    public void CalculatePopularityScore_ShouldReturnCorrectPopularityValue()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book1 = BookFactory.CreateWithFakeData();
        var book2 = BookFactory.CreateWithFakeData();

        // assign three borrow and reviews to book1
        for (int i = 0; i < 3; i++)
        {
            book1.Borrows.Add(BorrowFactory.CreateWithFakeData(book1, user));
            book1.BookReviews.Add(BookReviewFactory.CreateWithFakeData(book1, user));
        }

        //assign one borrow and review to book2
        book2.Borrows.Add(BorrowFactory.CreateWithFakeData(book2, user));
        book2.BookReviews.Add(BookReviewFactory.CreateWithFakeData(book2, user));

        // act & assert
        var book1Popularity = _calculator.CalculatePopularityScore(book1);
        var book2Popularity = _calculator.CalculatePopularityScore(book2);

        book1Popularity.Should().NotBe(0);
        book2Popularity.Should().NotBe(0);

        book1Popularity.Should().NotBe(book2Popularity);
        book1Popularity.Should().BeGreaterThan(book2Popularity);
    }
}
