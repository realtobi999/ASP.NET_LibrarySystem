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

    [Fact]
    public async void GenreController_GetGenres_Returns200AndLimitAndOffsetWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var genre1 = new Genre().WithFakeData();
        var genre2 = new Genre().WithFakeData();
        var genre3 = new Genre().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/genre", genre1.ToCreateGenreDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/genre", genre2.ToCreateGenreDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
 
        var create3 = await client.PostAsJsonAsync("/api/genre", genre3.ToCreateGenreDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync(string.Format("/api/genre?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<GenreDto>>() ?? throw new DeserializationException();
        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().BeEquivalentTo(genre2.ToDto());
        content.ElementAt(1).Should().BeEquivalentTo(genre3.ToDto());
    }
}
