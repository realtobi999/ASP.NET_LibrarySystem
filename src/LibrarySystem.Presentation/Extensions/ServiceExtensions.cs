using LibrarySystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibrarySystemContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("LibrarySystem"));
        });
    }
}