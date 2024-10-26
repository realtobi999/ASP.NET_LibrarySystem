using LibrarySystem.Application.Services.Books;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Tests.Integration.Helpers;

namespace LibrarySystem.Tests.Unit.Books;

public class BookPopularityCalculatorTests
{
    private readonly IBookPopularityCalculator _calculator;

    public BookPopularityCalculatorTests()
    {
        _calculator = new BookPopularityCalculator();
    }

    [Fact]
    public void CalculatePopularityScore_ShouldReturnCorrectPopularityValue()
    {
        // prepare
        var user = new User().WithFakeData();
        var book1 = new Book().WithFakeData();
        var book2 = new Book().WithFakeData();

        // assign three borrow and reviews to book1
        for (int i = 0; i < 3; i++)
        {
            book1.Borrows.Add(new Borrow().WithFakeData(book1, user));
            book1.BookReviews.Add(new BookReview().WithFakeData(book1, user));
        }

        //assign one borrow and review to book2
        book2.Borrows.Add(new Borrow().WithFakeData(book2, user));
        book2.BookReviews.Add(new BookReview().WithFakeData(book2, user));

        // act & assert
        var book1Popularity = _calculator.CalculatePopularityScore(book1);
        var book2Popularity = _calculator.CalculatePopularityScore(book2);

        book1Popularity.Should().NotBe(0);
        book2Popularity.Should().NotBe(0);

        book1Popularity.Should().NotBe(book2Popularity);
        book1Popularity.Should().BeGreaterThan(book2Popularity);
    }
}
