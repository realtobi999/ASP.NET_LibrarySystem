using LibrarySystem.Application.Core.Validators;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Helpers;
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
    public async void ValidateAsync_ReturnsFalseWhenUserDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData();
        var wishlist = new Wishlist().WithFakeData(user);

        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync((User?)null);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(wishlist);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested User with the key '{user.Id}' was not found in the system.");
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenBooksDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData();
        var wishlist = new Wishlist().WithFakeData(user);
        var book = new Book().WithFakeData();

        wishlist.Books = [
            new Book()
            {
                Id = book.Id,
            }
        ];

        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync(user);
        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync((Book?)null);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(wishlist);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Book with the key '{book.Id}' was not found in the system.");
    }
}
