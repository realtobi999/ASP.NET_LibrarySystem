using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Genres;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class GenreControllerTests
{
    [Fact]
    public async void GenreController_CreateGenre_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var genre = new Genre().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        // act & assert
        var response = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/api/genre/{0}", genre.Id));
    }

    [Fact]
    public async void GenreController_GetGenre_Returns201AndCorrectGenre()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var genre = new Genre().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create = await client.PostAsJsonAsync("/api/genre", genre.ToCreateGenreDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/genre/{0}", genre.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<GenreDto>() ?? throw new DeserializationException();
        content.Should().BeEquivalentTo(genre.ToDto());

        // test for 404
        var get = await client.GetAsync(string.Format("/api/genre/{0}", Guid.NewGuid()));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
