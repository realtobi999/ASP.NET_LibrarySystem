using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Entities.Relationships;
using LibrarySystem.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Infrastructure;

public class LibrarySystemContext(DbContextOptions<LibrarySystemContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BookAuthor> BookAuthor { get; set; }
    public DbSet<BookGenre> BookGenre { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employee { get; set; }

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
    }
}