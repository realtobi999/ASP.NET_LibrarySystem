using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigurePictureRelationships(this ModelBuilder builder)
    {
        // configure one-to-one relationship between Author and Picture
        builder.Entity<Picture>()
            .HasOne(p => p.Author)
            .WithOne(a => a.ProfilePicture)
            .HasForeignKey<Picture>(p => p.EntityId)
            .HasPrincipalKey<Author>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // configure one-to-one relationship between User and Picture
        builder.Entity<Picture>()
            .HasOne(p => p.User)
            .WithOne(a => a.ProfilePicture)
            .HasForeignKey<Picture>(p => p.EntityId)
            .HasPrincipalKey<User>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // configure one-to-many relationship between Book and Picture
        builder.Entity<Picture>()
            .HasOne(p => p.Book)
            .WithMany(b => b.CoverPictures)
            .HasForeignKey(p => p.EntityId)
            .OnDelete(DeleteBehavior.Cascade);

        // discriminator for entity type
        builder.Entity<Picture>()
            .HasDiscriminator<PictureEntityType>("entity_type")
            .HasValue<Picture>(PictureEntityType.Book)
            .HasValue<Picture>(PictureEntityType.Author)
            .HasValue<Picture>(PictureEntityType.User);
    }
}
