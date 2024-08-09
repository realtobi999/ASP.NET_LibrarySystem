using System.Net.Mail;
using LibrarySystem.Application.Core.Factories;
using LibrarySystem.Application.Services.Books;
using LibrarySystem.Application.Services.Wishlists;
using LibrarySystem.Domain.Interfaces.Emails;
using LibrarySystem.Domain.Interfaces.Utilities;
using LibrarySystem.EmailService;
using LibrarySystem.Infrastructure.Factories;
using LibrarySystem.Presentation.Extensions;
using LibrarySystem.Presentation.Middlewares.Filters;

namespace LibrarySystem.Presentation;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            var config = builder.Configuration;

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureCors();
            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<CustomDtoSerializationFilter>();
            });

            builder.Services.ConfigureDbContext(config.GetConnectionString("LibrarySystem"));

            // services
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.AddScoped<IBookAssociations, BookAssociations>();
            builder.Services.AddScoped<IWishlistAssociations, WishlistAssociations>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            // email clients
            builder.Services.AddSingleton(p => SmtpFactory.CreateInstance(config));
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.ConfigureMessageBuilders();
            builder.Services.ConfigureEmailManager();

            builder.Services.ConfigureJwtAuthentication(config); 
            builder.Services.AddAuthorizationBuilder()
                            .AddPolicy("User", policy => policy.RequireRole("User")) // user authorization
                            .AddPolicy("Employee", policy => policy.RequireRole("Employee")) // employee authorization
                            .AddPolicy("Admin", policy => policy.RequireRole("Admin")); // admin authorization
        }

        // application pipeline
        var app = builder.Build();
        {
            app.ConfigureExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MainCorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // custom middlewares
            app.UseUserAuthentication();
            app.UseEmployeeAuthentication();

            app.MapControllers();
            app.Run();
        }
    }
}