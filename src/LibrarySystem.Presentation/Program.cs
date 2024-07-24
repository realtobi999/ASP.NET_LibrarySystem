using System.Net.Mail;
using LibrarySystem.Application.Core.Factories;
using LibrarySystem.Application.Services.Books;
using LibrarySystem.Application.Services.Wishlists;
using LibrarySystem.Domain.Interfaces.Emails;
using LibrarySystem.Domain.Interfaces.Utilities;
using LibrarySystem.EmailService;
using LibrarySystem.Infrastructure.Factories;
using LibrarySystem.Presentation.Extensions;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            builder.Services.ConfigureCors();
            builder.Services.ConfigureDbContext(builder.Configuration.GetConnectionString("LibrarySystem"));

            // services
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();

            var jwt = JwtFactory.CreateInstance(builder.Configuration);
            builder.Services.AddSingleton<IJwt>(p => jwt);
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.ConfigureJwtAuthentication(jwt);

            // email clients
            var SMPT = SmtpFactory.CreateInstance(builder.Configuration);
            builder.Services.AddSingleton<SmtpClient>(p => SMPT);
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.ConfigureMessageBuilders();
            builder.Services.ConfigureEmailManager();


            builder.Services.AddScoped<IBookAssociations, BookAssociations>();
            builder.Services.AddScoped<IWishlistAssociations, WishlistAssociations>();
            
            // user authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });

            // employee authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
            });

            // admin authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
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