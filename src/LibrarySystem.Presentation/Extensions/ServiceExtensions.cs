using System.Text;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Interfaces.Repositories;
using LibrarySystem.Infrastructure.Factories;
using LibrarySystem.Infrastructure.Messages.Borrows;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LibrarySystem.Application.Core.Emails;
using LibrarySystem.Application.Core.Factories;
using LibrarySystem.Domain.Interfaces.Emails;
using LibrarySystem.Domain.Interfaces.Utilities;
using LibrarySystem.Infrastructure.Persistence.Repositories;
using LibrarySystem.Infrastructure.Persistence;

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

    public static void ConfigureJwtAuthentication(this IServiceCollection services, IJwt jwt)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt.Issuer,
                ValidAudience = jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
            };
        });
    }

    public static void ConfigureMessageBuilders(this IServiceCollection services)
    {
        services.AddSingleton<IBorrowMessageBuilder, BorrowMessageBuilder>();
    }

    public static void ConfigureEmailManager(this IServiceCollection services)
    {
        services.AddScoped<IEmailFactory, EmailFactory>();
        services.AddScoped<IEmailManager, EmailManager>();
    } 
}