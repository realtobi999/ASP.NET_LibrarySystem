using LibrarySystem.Application.Core.Validators;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Validators;

public class WishlistValidatorTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly WishlistValidator _validator;

    public WishlistValidatorTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _validator = new WishlistValidator(_repository.Object);
    }

    [Fact]
    public async Task ValidateAsync_ReturnsFalseWhenUserDoesntExist()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var wishlist = WishlistFactory.CreateWithFakeData(user);

        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync((User?)null);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(wishlist);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested User with the key '{user.Id}' was not found in the system.");
    }

    [Fact]
    public async Task ValidateAsync_ReturnsFalseWhenBooksDoesntExist()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var wishlist = WishlistFactory.CreateWithFakeData(user);
        var book = BookFactory.CreateWithFakeData();

        wishlist.Books = [book];

        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync(user);
        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync((Book?)null);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(wishlist);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Book with the key '{book.Id}' was not found in the system.");
    }
}