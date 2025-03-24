using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Tests.Integration.Server;

public static class TestServicesExtensions
{
    public static void RemoveService<TService>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TService));

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }

    public static void ReplaceWithInMemoryDatabase<TContext>(this IServiceCollection services, string dbName) where TContext : DbContext
    {
        services.RemoveService<DbContextOptions<TContext>>();

        services.AddDbContext<TContext>(options => { options.UseInMemoryDatabase(dbName); });
    }
}