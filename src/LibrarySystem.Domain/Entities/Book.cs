using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Entities;

public class Book : IDtoSerialization<BookDto>
{
    // core properties

    [Required, Column("id")]
    public required Guid Id { get; init; }

    [Required, Column("isbn")]
    public required string Isbn { get; init; }

    [Required, Column("title")]
    public required string Title { get; set; }

    [Required, Column("description")]
    public required string Description { get; set; }

    [Required, Column("pages_count")]
    public required int PagesCount { get; set; }

    [Required, Column("published_at")]
    public required DateTimeOffset PublishedDate { get; set; }

    [Required, Column("popularity")]
    public required double Popularity { get; set; }

    [Required, Column("available")]
    public required bool IsAvailable { get; set; }

    [Required, Column("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    // constants

    public const double POPULARITY_DEFAULT_VALUE = 100;

    // relationships

    [JsonIgnore]
    public virtual ICollection<Picture> CoverPictures { get; set; } = [];

    [JsonIgnore]
    public virtual ICollection<Author> Authors { get; set; } = [];

    [JsonIgnore]
    public virtual ICollection<Genre> Genres { get; set; } = [];

    [JsonIgnore]
    public virtual ICollection<BookReview> BookReviews { get; set; } = [];

    [JsonIgnore]
    public virtual ICollection<Borrow> Borrows { get; set; } = [];

    /// <inheritdoc/>
    public BookDto ToDto()
    {
        var authors = this.Authors.Select(a => a.ToDto()).ToList();
        var genres = this.Genres.Select(g => g.ToDto()).ToList();
        var reviews = this.BookReviews.Select(br => br.ToDto()).ToList();

        return new BookDto
        {
            Id = this.Id,
            Isbn = this.Isbn,
            Title = this.Title,
            Description = this.Description,
            PagesCount = this.PagesCount,
            PublishedDate = this.PublishedDate,
            IsAvailable = this.IsAvailable,
            CoverPictures = [.. this.CoverPictures],
            Authors = authors,
            Genres = genres,
            Reviews = reviews
        };
    }

    public void Update(UpdateBookDto dto, IEnumerable<Genre> genres, IEnumerable<Author> authors)
    {
        Title = dto.Title;
        Description = dto.Description;
        PagesCount = dto.PagesCount;
        PublishedDate = dto.PublishedDate;

        if (dto.Availability is not null)
        {
            IsAvailable = (bool)dto.Availability;
        }

        // clean previous attached genres, authors and assign new
        this.Genres.Clear();
        this.Authors.Clear();

        foreach (var genre in genres)
        {
            this.Genres.Add(genre);
        }

        foreach (var author in authors)
        {
            this.Authors.Add(author);
        }
    }

    public void UpdateIsAvailable(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }

    public void UpdatePopularity(double popularity)
    {
        Popularity = popularity;
    }
}