using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Interfaces;
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
            builder.Services.ConfigureDbContext(builder.Configuration);

            // services
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureJwtAuthentication(builder.Configuration);
            builder.Services.ConfigureJwtToken(builder.Configuration);
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            // user authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireRole("User"));
            });

            // staff authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Staff", policy => policy.RequireRole("Staff"));
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

            app.MapControllers();
            app.Run();
        }
    }
}