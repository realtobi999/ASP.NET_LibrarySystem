using LibrarySystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Tests.Integration.Server;

public class WebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.ReplaceWithInMemoryDatabase<LibrarySystemContext>(_dbName);
        });
    }

    /// <summary>
    /// Retrieves the <c>BankSystemContext</c> instance.
    /// </summary>
    /// <returns>The <c>BankSystemContext</c> instance used for database operations</returns>
    public LibrarySystemContext GetDatabaseContext()
    {
        var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LibrarySystemContext>();
        return context;
    }
}


