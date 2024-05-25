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
            builder.Services.ConfigureJwtToken(builder.Configuration);
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
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

            app.MapControllers();
            app.Run();
        }
    }
}