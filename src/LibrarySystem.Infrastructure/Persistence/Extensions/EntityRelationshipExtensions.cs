using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Extensions;

public static class EntityRelationshipExtensions
{
    public static void ConfigureWishlistBookRelationship(this ModelBuilder builder)
    {
        // configure many-to-many relationship between Wishlist and Book
        builder.Entity<WishlistBook>()
            .HasKey(wb => new { wb.WishlistId, wb.BookId });

        builder.Entity<WishlistBook>()
            .HasOne(wb => wb.Wishlist)
            .WithMany(w => w.WishlistBooks)
            .HasForeignKey(wb => wb.WishlistId);

        builder.Entity<WishlistBook>()
            .HasOne(wb => wb.Book)
            .WithMany()
            .HasForeignKey(wb => wb.BookId);   
    }

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

    public static void ConfigureBookReviewsRelationShip(this ModelBuilder builder)
    {
        // configure one-to-many relationship between Book and BookReview
        builder.Entity<Book>()
            .HasMany(b => b.BookReviews)
            .WithOne(br => br.Book)
            .HasForeignKey(br => br.BookId);
    }
}
