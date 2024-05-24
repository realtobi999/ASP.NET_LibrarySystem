using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Domain.Entities.Relationships;

public class BookAuthor
{
    [Required, Column("book_id")]
    public Guid BookId { get; set; }
    public Book? Book { get; set; }

    [Required, Column("author_id")]
    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }
}
