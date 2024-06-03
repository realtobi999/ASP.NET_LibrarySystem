using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests;

public class AuthorControllerTests
{
    [Fact]
    public async void AuthorController_GetAuthors_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author1 = new Author().WithFakeData();
        var author2 = new Author().WithFakeData();
        var author3 = new Author().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/author", author1.ToCreateAuthorDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/author", author2.ToCreateAuthorDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/author", author3.ToCreateAuthorDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);      

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync(string.Format("/api/author?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<AuthorDto>>() ?? throw new DeserializationException();
        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().Be(author2.ToDto());
    }

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

    [Fact]
    public async void AuthorController_GetAuthor_Returns200AndCorrectAuthor()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author = new Author().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
 
        // act & assert
        var get1 = await client.GetAsync(string.Format("/api/author/{0}", author.Id));
        get1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get2 = await client.GetAsync(string.Format("/api/author/{0}", Guid.NewGuid()));
        get2.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

        var content = await get1.Content.ReadFromJsonAsync<AuthorDto>() ?? throw new DeserializationException();
        content.Should().Be(author.ToDto());
    }

    [Fact]
    public async void AuthorController_UpdateAuthor_Returns200AndCheckIfTheAuthorIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author = new Author().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateAuthorDto
        {
            Name = "test",
            Description = "test_test_test",
            Birthday = DateTimeOffset.UtcNow.AddDays(2),
            ProfilePicture = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5 }) // imagine this is the encoded string
        };

        var response = await client.PutAsJsonAsync(string.Format("/api/author/{0}", author.Id), updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/author/{0}", author.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<AuthorDto>() ?? throw new DeserializationException(); 
        content.Id.Should().Be(author.Id);
        content.Name.Should().Be(updateDto.Name);
        content.Description.Should().Be(updateDto.Description);
        content.Birthday.Should().Be(updateDto.Birthday);
        content.ProfilePicture.Should().Be(updateDto.ProfilePicture);
    }

    [Fact]
    public async void AuthorController_DeleteAuthor_Returns200AndCheckIfItExists()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var author = new Author().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create = await client.PostAsJsonAsync("/api/author", author.ToCreateAuthorDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync(string.Format("/api/author/{0}", author.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/author/{0}", author.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
