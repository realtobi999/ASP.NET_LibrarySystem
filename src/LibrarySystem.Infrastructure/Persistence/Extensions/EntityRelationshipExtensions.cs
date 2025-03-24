using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Extensions;

public static class EntityRelationshipExtensions
{
    public static void ConfigureUserRelationships(this ModelBuilder builder)
    {
        // configure one-to-many relationship between User and BookReviews
        builder.Entity<User>()
            .HasMany(u => u.BookReviews)
            .WithOne(br => br.User)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // configure one-to-many relationship between User and Borrow
        builder.Entity<User>()
            .HasMany(u => u.Borrows)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // configure one-to-many relationship between User and Wishlist
        builder.Entity<User>()
            .HasMany(u => u.Wishlists)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigureBookRelationship(this ModelBuilder builder)
    {
        // configure many-to-many relationship between Book and Author 
        builder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books);

        // configure many-to-many relationship between Book and Genre 
        builder.Entity<Book>()
            .HasMany(b => b.Genres)
            .WithMany(g => g.Books);

        // configure one-to-many relationship between Book and Borrow
        builder.Entity<Book>()
            .HasMany(b => b.Borrows)
            .WithOne(b => b.Book)
            .HasForeignKey(b => b.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // configure one-to-many relationship between Book and BookReviews
        builder.Entity<Book>()
            .HasMany(b => b.BookReviews)
            .WithOne(br => br.Book)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Book>()
            .Navigation(b => b.Authors)
            .AutoInclude();
        builder.Entity<Book>()
            .Navigation(b => b.Genres)
            .AutoInclude();
        builder.Entity<Book>()
            .Navigation(b => b.Borrows)
            .AutoInclude();
        builder.Entity<Book>()
            .Navigation(b => b.BookReviews)
            .AutoInclude();
        builder.Entity<Book>()
            .Navigation(b => b.CoverPictures)
            .AutoInclude();
    }

    public static void ConfigureWishlistRelationship(this ModelBuilder builder)
    {
        // configure many-to-many relationship between Wishlist and Book
        builder.Entity<Wishlist>()
            .HasMany(w => w.Books);
    }

    public static void ConfigurePictureRelationships(this ModelBuilder builder)
    {
        // configure one-to-one relationship between Author and Picture
        builder.Entity<Picture>()
            .HasOne(p => p.Author)
            .WithOne(a => a.Picture)
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

        // configure one-to-one relationship between Employee and Picture
        builder.Entity<Picture>()
            .HasOne(p => p.Employee)
            .WithOne(a => a.Picture)
            .HasForeignKey<Picture>(p => p.EntityId)
            .HasPrincipalKey<Employee>(a => a.Id)
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
            .HasValue<Picture>(PictureEntityType.User)
            .HasValue<Picture>(PictureEntityType.Employee);
    }
}