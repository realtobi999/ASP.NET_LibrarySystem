using LibrarySystem.Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Extensions;

public static class EntityRelationshipExtensions
{
    public static void ConfigureBookAuthorRelationship(this ModelBuilder builder)
    {
        // configure many-to-many relationship between Book and Author
        builder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);

        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany()
            .HasForeignKey(ba => ba.AuthorId);
    }

    public static void ConfigureBookGenreRelationship(this ModelBuilder builder)
    {
        // configure many-to-many relationship between Book and Genre
        builder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        builder.Entity<BookGenre>()
            .HasOne(bg => bg.Book)
            .WithMany(b => b.BookGenres)
            .HasForeignKey(bg => bg.BookId);

        builder.Entity<BookGenre>()
            .HasOne(bg => bg.Genre)
            .WithMany()
            .HasForeignKey(bg => bg.GenreId);
    }
}
