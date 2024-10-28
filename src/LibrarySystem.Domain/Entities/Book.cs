using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibrarySystem.Domain.Dtos.Books;
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

    [Required, Column("popularity")]
    public double Popularity { get; set; }

    [Required, Column("available")]
    public bool IsAvailable { get; set; }

    [Required, Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    // constants

    public const double POPULARITY_DEFAULT_VALUE = 100;

    // relationships

    [JsonIgnore]
    public ICollection<Picture> CoverPictures { get; set; } = [];
    
    [JsonIgnore]
    public ICollection<Author> Authors { get; set; } = [];

    [JsonIgnore]
    public ICollection<Genre> Genres { get; set; } = [];

    [JsonIgnore]
    public ICollection<BookReview> BookReviews { get; set; } = [];

    [JsonIgnore]
    public ICollection<Borrow> Borrows { get; set; } = [];

    /// <inheritdoc/>
    public BookDto ToDto()
    {
        var authors = this.Authors.Select(a => a.ToDto()).ToList();
        var genres =  this.Genres.Select(g => g.ToDto()).ToList();
        var reviews = this.BookReviews.Select(br => br.ToDto()).ToList();

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

    public void UpdateAvailability(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }

    public void UpdatePopularity(double popularity)
    {
        Popularity = popularity;
    }
}
