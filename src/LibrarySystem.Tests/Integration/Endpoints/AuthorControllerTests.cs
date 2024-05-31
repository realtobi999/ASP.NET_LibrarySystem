using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests;

public class AuthorControllerTests
{
    [Fact]
    public async void AuthorController_CreateAuthor_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author = new Author().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        // act & assert
        var response = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/api/author/{0}", author.Id));
    }
}
