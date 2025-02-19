using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class GenreControllerTests
{
    [Fact]
    public async void GetGenres_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var genre1 = GenreFactory.CreateWithFakeData();
        var genre2 = GenreFactory.CreateWithFakeData();
        var genre3 = GenreFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/genre", genre3.ToCreateGenreDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync($"/api/genre?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<GenreDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(genre2.Id);
        content.ElementAt(1).Id.Should().Be(genre3.Id);
    }

    [Fact]
    public async void GetGenre_Returns200AndCorrectValue()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var genre = GenreFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/genre/{genre.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GenreDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(genre.Id);

        var get = await client.GetAsync($"/api/genre/{Guid.NewGuid()}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async void CreateGenre_Returns201AndGenreIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var genre = GenreFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // act & assert
        var response = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/genre/{genre.Id}");

        // assert that genre is created
        using var context = app.GetDatabaseContext();
        context.Set<Genre>().Any(g => g.Id == genre.Id).Should().BeTrue();
    }


    [Fact]
    public async void UpdateGenre_Returns204AndGenreIsUpdated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var genre = GenreFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateGenreDto
        {
            Name = "test",
        };

        var response = await client.PutAsJsonAsync($"/api/genre/{genre.Id}", updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // assert that genre is updated
        using var context = app.GetDatabaseContext();
        var content = context.Set<Genre>().FirstOrDefault(g => g.Id == genre.Id) ?? throw new NullReferenceException();

        content.Id.Should().Be(genre.Id);
        content.Name.Should().Be(updateDto.Name);
    }

    [Fact]
    public async void DeleteGenre_Returns204AndGenreIsDeleted()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var genre = GenreFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"/api/genre/{genre.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        // assert that genre is deleted
        using var context = app.GetDatabaseContext();
        context.Set<Genre>().Any(g => g.Id == genre.Id).Should().BeFalse();
    }
}
