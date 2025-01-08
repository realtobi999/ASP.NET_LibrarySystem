using LibrarySystem.Application.Core.Validators;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Validators;

public class BookReviewValidatorTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly BookReviewValidator _validator;

    public BookReviewValidatorTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _validator = new BookReviewValidator(_repository.Object);
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenBookDoesntExist()
    {
        // prepare
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);

        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync((Book?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(review);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Book with the key '{book.Id}' was not found in the system.");
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenUserDoesntExist()
    {
        // prepare
        var book = BookFactory.CreateWithFakeData();
        var user = UserFactory.CreateWithFakeData();
        var review = BookReviewFactory.CreateWithFakeData(book, user);

        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync(book);
        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync((User?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(review);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested User with the key '{user.Id}' was not found in the system.");
    }
}
