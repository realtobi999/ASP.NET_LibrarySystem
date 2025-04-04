using System.Security.Cryptography;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Emails;
using LibrarySystem.EmailService;
using LibrarySystem.Presentation.Extensions;
using LibrarySystem.Presentation.Middlewares;
using LibrarySystem.Presentation.Middlewares.Filters;
using LibrarySystem.Presentation.Middlewares.Handlers;
using LibrarySystem.Presentation.Services;

namespace LibrarySystem.Presentation;

public class Program
{
    public const string NAME = "LibrarySystem";
    public const string VERSION = "1.0.0";

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            var config = builder.Configuration;

            builder.Services.AddHttpLogging(_ => { });
            builder.Services.ConfigureSwagger();

            builder.Services.ConfigureCors();
            builder.Services.AddControllers(opt => { opt.Filters.Add<CustomDtoSerializationFilter>(); });

            builder.Services.ConfigureDbContext(config.GetConnectionString("LibrarySystem"));

            // services
            builder.Services.AddHostedService<BookPopularityBackgroundService>();

            builder.Services.ConfigureFactories();
            builder.Services.ConfigureManagers();
            builder.Services.ConfigureValidators();
            builder.Services.ConfigureMappers();
            builder.Services.AddSingleton<IHasher>(new Hasher(algorithm: HashAlgorithmName.SHA512));

            // email client
            builder.Services.AddSingleton(_ => SmtpFactory.CreateInstance(config));
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.ConfigureMessageBuilders();
            builder.Services.ConfigureEmailManager();

            builder.Services.ConfigureJwtAuthentication(config);
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("User", policy => policy.RequireRole("User")) // user authorization
                .AddPolicy("Employee", policy => policy.RequireRole("Employee")) // employee authorization
                .AddPolicy("Admin", policy => policy.RequireRole("Admin")); // admin authorization
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        }

        // application pipeline
        var app = builder.Build();
        {
            app.UseHttpLogging();
            app.UseExceptionHandler(_ => { });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(opt => { opt.SwaggerEndpoint($"/swagger/{VERSION}/swagger.json", NAME); });
            }

            app.UseCors("MainCorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // custom middlewares
            app.UseMiddleware<UserAuthenticationMiddleware>();
            app.UseMiddleware<EmployeeAuthenticationMiddleware>();

            app.MapControllers();
            app.Run();
        }
    }
}