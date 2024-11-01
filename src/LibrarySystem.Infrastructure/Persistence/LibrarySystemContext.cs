using LibrarySystem.Domain.Entities;
using LibrarySystem.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure.Persistence;

public class LibrarySystemContext(DbContextOptions<LibrarySystemContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BookReview> BookReviews { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Borrow> Borrows { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<Picture> Pictures { get; set; }

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
        modelBuilder.ConfigureBookRelationship();
        modelBuilder.ConfigurePictureRelationships();
        modelBuilder.ConfigureUserRelationships();
        modelBuilder.ConfigureWishlistRelationship();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

}