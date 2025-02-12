﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Authors;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class AuthorControllerTests
{
    [Fact]
    public async void GetAuthors_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author1 = AuthorFactory.CreateWithFakeData();
        var author2 = AuthorFactory.CreateWithFakeData();
        var author3 = AuthorFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create1 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author3.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync($"/api/author?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<AuthorDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().BeEquivalentTo(author2.ToDto());
    }

    [Fact]
    public async void GetAuthor_Returns200AndCorrectValue()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author = AuthorFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var get = await client.GetAsync($"/api/author/{author.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<AuthorDto>() ?? throw new NullReferenceException();

        content.Should().BeEquivalentTo(author.ToDto());
    }

    [Fact]
    public async void CreateAuthor_Returns201AndAuthorIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var author = AuthorFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // act & assert
        var response = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/author/{author.Id}");

        // assert that the author is created
        using var context = app.GetDatabaseContext();
        context.Set<Author>().Any(a => a.Id == author.Id).Should().BeTrue();
    }

    [Fact]
    public async void UpdateAuthor_Returns204AndAuthorIsUpdated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var author = AuthorFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateAuthorDto
        {
            Name = "test",
            Description = "test_test_test",
            Birthday = DateTimeOffset.UtcNow.AddDays(2),
        };

        var response = await client.PutAsJsonAsync($"/api/author/{author.Id}", updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // assert that the author is updated
        using var context = app.GetDatabaseContext();
        var updatedAuthor = context.Set<Author>().FirstOrDefault(a => a.Id == author.Id) ?? throw new NullReferenceException();

        updatedAuthor.Id.Should().Be(author.Id);
        updatedAuthor.Name.Should().Be(updateDto.Name);
        updatedAuthor.Description.Should().Be(updateDto.Description);
        updatedAuthor.Birthday.Should().Be(updateDto.Birthday);
    }

    [Fact]
    public async void DeleteAuthor_Returns204AndAuthorIsDeleted()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var author = AuthorFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/author/{author.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // assert that the author is created
        using var context = app.GetDatabaseContext();
        context.Set<Author>().Any(a => a.Id == author.Id).Should().BeFalse();
    }

    [Fact]
    public async void UploadPhoto_Returns204AndPhotoIsUploaded()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var author = AuthorFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // make the file
        var photo1 = new ByteArrayContent([1, 2, 3, 4]);
        photo1.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        var formData = new MultipartFormDataContent
        {
            { photo1, "file", "photo1.jpg" },
        };

        // act & assert
        var response = await client.PutAsync($"/api/author/{author.Id}/photo", formData);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // assert that the user profile picture is updated
        using var context = app.GetDatabaseContext();
        var updatedAuthor = context.Set<Author>().Include(a => a.Picture).FirstOrDefault(a => a.Id == author.Id) ?? throw new NullReferenceException();

        updatedAuthor.Id.Should().Be(author.Id);
        updatedAuthor.Picture.Should().NotBeNull();
        updatedAuthor.Picture!.FileName.Should().Be("photo1.jpg");
    }
}
