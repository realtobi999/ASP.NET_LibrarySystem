using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Entities.Relationships;

public class BookGenre
{
    [Required, Column("book_id")]
    public Guid BookId { get; set; }
    public Book? Book { get; set; }

    [Required, Column("genre_id")]
    public Guid GenreId { get; set; }
    public Genre? Genre { get; set; }
}