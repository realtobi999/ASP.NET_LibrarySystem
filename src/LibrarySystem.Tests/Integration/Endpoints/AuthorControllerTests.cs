using System.Net;
using System.Net.Http.Headers;
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
    public async Task GetAuthors_Returns200AndCorrectValues()
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
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author3.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        const int limit = 2;
        const int offset = 1;

        var response = await client.GetAsync($"/api/author?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<AuthorDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(author2.Id);
    }

    [Fact]
    public async Task GetAuthor_Returns200AndCorrectValue()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author = AuthorFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var get = await client.GetAsync($"/api/author/{author.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<AuthorDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(author.Id);
    }

    [Fact]
    public async Task CreateAuthor_Returns201AndAuthorIsCreated()
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
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/author/{author.Id}");

        // assert that the author is created
        await using var context = app.GetDatabaseContext();
        context.Set<Author>().Any(a => a.Id == author.Id).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAuthor_Returns204AndAuthorIsUpdated()
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
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateAuthorDto
        {
            Name = "test",
            Description = "test_test_test",
            Birthday = DateTimeOffset.UtcNow.AddDays(2)
        };

        var response = await client.PutAsJsonAsync($"/api/author/{author.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the author is updated
        await using var context = app.GetDatabaseContext();
        var updatedAuthor = context.Set<Author>().FirstOrDefault(a => a.Id == author.Id) ?? throw new NullReferenceException();

        updatedAuthor.Name.Should().Be(updateDto.Name);
        updatedAuthor.Description.Should().Be(updateDto.Description);
        updatedAuthor.Birthday.Should().Be(updateDto.Birthday);
    }

    [Fact]
    public async Task DeleteAuthor_Returns204AndAuthorIsDeleted()
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
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/author/{author.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the author is created
        await using var context = app.GetDatabaseContext();
        context.Set<Author>().Any(a => a.Id == author.Id).Should().BeFalse();
    }

    [Fact]
    public async Task UploadPhoto_Returns204AndPhotoIsUploaded()
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
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        // make the file
        var photo1 = new ByteArrayContent([1, 2, 3, 4]);
        photo1.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        var formData = new MultipartFormDataContent
        {
            { photo1, "file", "photo1.jpg" }
        };

        // act & assert
        var response = await client.PutAsync($"/api/author/{author.Id}/photo", formData);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the user profile picture is uploaded
        await using var context = app.GetDatabaseContext();
        var updatedAuthor = context.Set<Author>().Include(a => a.Picture).FirstOrDefault(a => a.Id == author.Id) ??
                            throw new NullReferenceException();

        updatedAuthor.Picture.Should().NotBeNull();
        updatedAuthor.Picture!.FileName.Should().Be("photo1.jpg");
    }
}