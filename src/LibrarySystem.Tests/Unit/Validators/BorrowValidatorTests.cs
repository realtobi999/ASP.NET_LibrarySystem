using LibrarySystem.Application.Core.Validators;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using Moq;

namespace LibrarySystem.Tests.Unit.Validators;

public class BorrowValidatorTests
{

    private readonly Mock<IRepositoryManager> _repository;
    private readonly BorrowValidator _validator;

    public BorrowValidatorTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _validator = new BorrowValidator(_repository.Object);
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenBookDoesntExistAsync()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);

        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync((Book?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(borrow);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Book with the key '{book.Id}' was not found in the system.");
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenUserDoesntExistAsync()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);

        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync(book);
        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync((User?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(borrow);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested User with the key '{user.Id}' was not found in the system.");
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenDueDateIsBeforeBorrowDate()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);

        // make the DueDate one month before the borrowDate
        borrow.BorrowDate = DateTimeOffset.Now;
        borrow.DueDate = DateTimeOffset.Now.AddMonths(-1);

        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync(book);
        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync(user);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(borrow);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"Borrow date must be before the due date.");
    }


    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenBookIsNotReturnedAfterDueDate()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);

        // make it so the borrow is one month late of returning
        borrow.BorrowDate = DateTimeOffset.Now.AddMonths(-2);
        borrow.DueDate = DateTimeOffset.Now.AddMonths(-1);
        borrow.IsReturned = false;

        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync(book);
        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync(user);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(borrow);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"The book cannot be set to returned after the due date.");
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenBookIsSetToAvailableAfterBorrowing()
    {
        // prepare
        var user = UserFactory.CreateWithFakeData();
        var book = BookFactory.CreateWithFakeData();
        var borrow = BorrowFactory.CreateWithFakeData(book, user);

        book.IsAvailable = true;

        _repository.Setup(r => r.Book.GetAsync(book.Id)).ReturnsAsync(book);
        _repository.Setup(r => r.User.GetAsync(user.Id)).ReturnsAsync(user);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(borrow);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<Conflict409Exception>();
        exception!.Message.Should().Be($"The book should be marked as unavailable if not returned.");
    }
}
