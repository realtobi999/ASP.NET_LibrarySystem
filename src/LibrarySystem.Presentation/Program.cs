using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Emails;
using LibrarySystem.EmailService;
using LibrarySystem.Presentation.Extensions;
using LibrarySystem.Presentation.Middlewares;
using LibrarySystem.Presentation.Middlewares.Filters;
using LibrarySystem.Presentation.Middlewares.Handlers;

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
            builder.Services.ConfigureFactories();
            builder.Services.ConfigureManagers();
            builder.Services.ConfigureValidators();
            builder.Services.ConfigureMappers();
            builder.Services.AddScoped<IHasher, Hasher>();

            // email client
            builder.Services.AddSingleton(p => SmtpFactory.CreateInstance(config));
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
            app.UseExceptionHandler(opt => { });

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
            app.UseMiddleware<UserAuthenticationMiddleware>();
            app.UseMiddleware<EmployeeAuthenticationMiddleware>();

            app.MapControllers();
            app.Run();
        }
    }
}