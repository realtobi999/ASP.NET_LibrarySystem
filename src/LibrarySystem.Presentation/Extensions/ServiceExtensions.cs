﻿using System.Text;
using LibrarySystem.Application.Services;
using LibrarySystem.Infrastructure.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LibrarySystem.Application.Core.Emails;
using LibrarySystem.Application.Core.Factories;
using LibrarySystem.Infrastructure.Persistence.Repositories;
using LibrarySystem.Infrastructure.Persistence;
using LibrarySystem.Domain.Interfaces.Common;
using LibrarySystem.Domain.Interfaces.Emails.Borrow;
using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Managers;
using LibrarySystem.Infrastructure.Messages.Builders;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Application.Core.Validators;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Wishlists;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Dtos.Borrows;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Dtos.Books;
using LibrarySystem.Application.Core.Mappers;
using Microsoft.OpenApi.Models;
using LibrarySystem.Presentation.Middlewares.Filters;

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

    public static void ConfigureDbContext(this IServiceCollection services, string? connection)
    {
        services.AddDbContext<LibrarySystemContext>(options =>
        {
            options.UseNpgsql(
                connection,
                builder =>
                {
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    builder.EnableRetryOnFailure(maxRetryCount: 3);
                });
        });
    }

    public static void ConfigureFactories(this IServiceCollection services)
    {
        services.AddScoped<IServiceFactory, ServiceFactory>();
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IMapperFactory, MapperFactory>();
        services.AddScoped<IValidatorFactory, ValidatorFactory>();
    }

    public static void ConfigureManagers(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IMapperManager, MapperManager>();
    }

    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = JwtFactory.CreateInstance(configuration);

        services.AddSingleton<IJwt>(_ => jwt);

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

    public static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Book>, BookValidator>();
        services.AddScoped<IValidator<BookReview>, BookReviewValidator>();
        services.AddScoped<IValidator<Borrow>, BorrowValidator>();
        services.AddScoped<IValidator<Wishlist>, WishlistValidator>();
    }

    public static void ConfigureMappers(this IServiceCollection services)
    {
        services.AddScoped<IMapper<Author, CreateAuthorDto>, AuthorMapper>();
        services.AddScoped<IMapper<Book, CreateBookDto>, BookMapper>();
        services.AddScoped<IMapper<BookReview, CreateBookReviewDto>, BookReviewMapper>();
        services.AddScoped<IMapper<Borrow, CreateBorrowDto>, BorrowMapper>();
        services.AddScoped<IMapper<Employee, RegisterEmployeeDto>, EmployeeMapper>();
        services.AddScoped<IMapper<Genre, CreateGenreDto>, GenreMapper>();
        services.AddScoped<IMapper<User, RegisterUserDto>, UserMapper>();
        services.AddScoped<IMapper<Wishlist, CreateWishlistDto>, WishlistMapper>();
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            // configure the swagger documentation
            opt.SwaggerDoc(Program.VERSION, new OpenApiInfo
            {
                Title = Program.NAME,
                Version = Program.VERSION,
                Description = "Swagger for a library system API.",
                Contact = new OpenApiContact
                {
                    Name = "Tobiáš Filgas",
                    Email = "tobiasfilgas.work@gmail.com"
                }
            });

            // configure the swagger authentication
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            // apply a filter so only the endpoints with authorize attribute are locked
            opt.OperationFilter<SwaggerAuthorizeOperationFilter>();
        });

        services.AddEndpointsApiExplorer();
    }
}