using LibrarySystem.Application.Core.Validators;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Tests.Integration.Helpers;
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
    public async void ValidateAsync_ReturnsFalseWhenAssignedGenresDoesntExist()
    {
        // prepare
        var book = new Book().WithFakeData();
        var genre = new Genre().WithFakeData();

        book.BookGenres = [new (){
            BookId = book.Id,
            Book = book,
            GenreId = genre.Id,
            Genre = genre,
        }];

        _repository.Setup(r => r.Genre.GetAsync(genre.Id)).ReturnsAsync((Genre?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(book);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Genre with the key '{genre.Id}' was not found in the system.");
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenAssignedAuthorsDoesntExist()
    {
        // prepare
        var book = new Book().WithFakeData();
        var genre = new Genre().WithFakeData();
        var author = new Author().WithFakeData();

        book.BookGenres = [new (){
            BookId = book.Id,
            Book = book,
            GenreId = genre.Id,
            Genre = genre,
        }];
        book.BookAuthors = [new (){
            BookId = book.Id,
            Book = book,
            AuthorId = author.Id,
            Author = author,
        }];

        _repository.Setup(r => r.Genre.GetAsync(genre.Id)).ReturnsAsync(genre);
        _repository.Setup(r => r.Author.GetAsync(author.Id)).ReturnsAsync((Author?)null); // set the return to null value => emulate that the repository couldn't find this entity

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(book);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested Author with the key '{author.Id}' was not found in the system.");
    }

    [Fact]
    public async void ValidateAsync_ReturnsFalseWhenBookReviewsAreWithWrongBookId()
    {
        // prepare
        var book = new Book().WithFakeData();
        var genre = new Genre().WithFakeData();
        var author = new Author().WithFakeData();

        var review = new BookReview().WithFakeData(book, new User().WithFakeData());
        review.BookId = Guid.NewGuid(); // assign a different book id

        book.BookGenres = [new (){
            BookId = book.Id,
            Book = book,
            GenreId = genre.Id,
            Genre = genre,
        }];
        book.BookAuthors = [new (){
            BookId = book.Id,
            Book = book,
            AuthorId = author.Id,
            Author = author,
        }];
        book.BookReviews = [review];

        _repository.Setup(r => r.Genre.GetAsync(genre.Id)).ReturnsAsync(genre);
        _repository.Setup(r => r.Author.GetAsync(author.Id)).ReturnsAsync(author); 

        // act & assert
        var (isValid, exception) = await _validator.ValidateAsync(book);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<Conflict409Exception>();
        exception!.Message.Should().Be($"Review with ID {review.Id} is incorrectly associated with Book ID {review.BookId} instead of Book ID {book.Id}.");
    }
}
