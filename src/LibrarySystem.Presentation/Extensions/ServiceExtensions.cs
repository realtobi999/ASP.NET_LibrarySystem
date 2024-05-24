using LibrarySystem.Application.Factories;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Infrastructure;
using LibrarySystem.Infrastructure.Factories;
using LibrarySystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("MainCorsPolicy", builder =>
                    builder
                    .AllowAnyOrigin()
                    .WithMethods("POST", "GET", "PUT", "DELETE"));
        });
    }

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LibrarySystemContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("LibrarySystem"));
        });
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }
}