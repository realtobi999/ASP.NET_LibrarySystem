﻿using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence;

public class LibrarySystemContext(DbContextOptions<LibrarySystemContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BookAuthor> BookAuthor { get; set; }
    public DbSet<BookGenre> BookGenre { get; set; }
    public DbSet<BookReview> BookReviews { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Borrow> Borrow { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<WishlistBook> WishlistBooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // properties 
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // relationships
        modelBuilder.ConfigureBookAuthorRelationship();
        modelBuilder.ConfigureBookGenreRelationship();
        modelBuilder.ConfigureBookReviewsRelationShip();
        modelBuilder.ConfigureWishlistBookRelationship();
    }
}