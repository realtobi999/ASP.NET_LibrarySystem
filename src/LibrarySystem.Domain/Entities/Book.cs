using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Book : IDtoSerialization<BookDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("isbn")]
    public string? ISBN { get; set; }

    [Required, Column("title")]
    public string? Title { get; set; }

    [Required, Column("description")]
    public string? Description { get; set; }

    [Required, Column("pages_count")]
    public int PagesCount { get; set; }

    [Required, Column("published_at")]
    public DateTimeOffset PublishedDate { get; set; }

    [Required, Column("available")]
    public bool IsAvailable { get; set; }

    // relationships

    [JsonIgnore]
    public ICollection<Picture> CoverPictures { get; set; } = [];
    [JsonIgnore]
    public ICollection<BookAuthor> BookAuthors { get; set; } = [];
    [JsonIgnore]
    public ICollection<BookGenre> BookGenres { get; set; } = [];
    [JsonIgnore]
    public ICollection<BookReview> BookReviews { get; set; } = [];

    /// <inheritdoc/>
    public BookDto ToDto()
    {
        var authors = this.BookAuthors.Where(ba => ba.Author is not null)
                        .Select(ba => ba.Author!.ToDto())
                        .ToList();

        var genres = this.BookGenres.Where(bg => bg.Genre is not null)
                        .Select(bg => bg.Genre!.ToDto())
                        .ToList();

        var reviews = this.BookReviews.Select(br => br.ToDto())
                                      .ToList();

        return new BookDto
        {
            Id = this.Id,
            ISBN = this.ISBN,
            Title = this.Title,
            Description = this.Description,
            PagesCount = this.PagesCount,
            PublishedDate = this.PublishedDate,
            IsAvailable = this.IsAvailable,
            CoverPictures = [.. this.CoverPictures],
            Authors = authors,
            Genres = genres,
            Reviews = reviews,
        };
    }
}
