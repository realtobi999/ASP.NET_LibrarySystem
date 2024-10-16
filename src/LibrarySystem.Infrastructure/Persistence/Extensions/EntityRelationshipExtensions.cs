﻿using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence.Extensions;

public static class EntityRelationshipExtensions
{
    public static void ConfigureWishlistRelationship(this ModelBuilder builder)
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

    public static void ConfigureBookRelationships(this ModelBuilder builder)
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

        // configure one-to-many relationship between Book and BookReview
        builder.Entity<Book>()
            .HasMany(b => b.BookReviews)
            .WithOne(br => br.Book)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void ConfigureUserRelationships(this ModelBuilder builder)
    {

        // configure one-to-many relationship between User and BookReview
        builder.Entity<User>()
            .HasMany(u => u.BookReviews)
            .WithOne(br => br.User)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // configure one-to-many relationship between User and Wishlist
        builder.Entity<User>()
            .HasMany(u => u.Wishlists)
            .WithOne(w => w.User)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // configure one-to-many relationship between User and borrows
        builder.Entity<User>()
            .HasMany(u => u.Borrows)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
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
