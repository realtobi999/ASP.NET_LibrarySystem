using LibrarySystem.Application.Core.Validators;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Tests.Integration.Factories;
using Moq;

namespace LibrarySystem.Tests.Unit.Validators;

public class BookValidatorTests
{
    private readonly Mock<IRepositoryManager> _repository;
    private readonly BookValidator _validator;

    public BookValidatorTests()
    {
        _repository = new Mock<IRepositoryManager>();
        _validator = new BookValidator(_repository.Object);
    }

    [Fact]
    public async Task ValidateAsync_ReturnsFalseWhenAssignedGenresDoesntExist()
    {
        // prepare
        var book = BookFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();
        var author = AuthorFactory.CreateWithFakeData();

        book.Genres = [genre];
        book.Authors = [author];

        _repository.Setup(r => r.Genre.GetAsync(genre.Id))
            .ReturnsAsync((Genre?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(book);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Genre with the key '{genre.Id}' was not found in the system.");
    }

    [Fact]
    public async Task ValidateAsync_ReturnsFalseWhenAssignedAuthorsDoesntExist()
    {
        // prepare
        var book = BookFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();
        var author = AuthorFactory.CreateWithFakeData();

        book.Genres = [genre];
        book.Authors = [author];

        _repository.Setup(r => r.Genre.GetAsync(genre.Id)).ReturnsAsync(genre);
        _repository.Setup(r => r.Author.GetAsync(author.Id))
            .ReturnsAsync((Author?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(book);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Author with the key '{author.Id}' was not found in the system.");
    }

    [Fact]
    public async Task ValidateAsync_ReturnsFalseWhenBookReviewsAreWithWrongBookId()
    {
        // prepare
        var book = BookFactory.CreateWithFakeData();
        var genre = GenreFactory.CreateWithFakeData();
        var author = AuthorFactory.CreateWithFakeData();

        var review = BookReviewFactory.CreateWithFakeData(book, UserFactory.CreateWithFakeData());
        review.BookId = Guid.NewGuid(); // assign a different book id

        book.Genres = [genre];
        book.Authors = [author];

        book.BookReviews = [review];

        _repository.Setup(r => r.Genre.GetAsync(genre.Id)).ReturnsAsync(genre);
        _repository.Setup(r => r.Author.GetAsync(author.Id)).ReturnsAsync(author);

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(book);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<Conflict409Exception>();
        exception!.Message.Should()
            .Be($"Review with ID {review.Id} is incorrectly associated with Book ID {review.BookId} instead of Book ID {book.Id}.");
    }
}