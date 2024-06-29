using LibrarySystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Infrastructure.Factories;

public class LibrarySystemContextFactory : IDesignTimeDbContextFactory<LibrarySystemContext>
{
    public LibrarySystemContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "../../LibrarySystem.Presentation")
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();
              
        var builder = new DbContextOptionsBuilder<LibrarySystemContext>().UseNpgsql(configuration.GetConnectionString("LibrarySystem"));

        return new LibrarySystemContext(builder.Options);
    }
}
