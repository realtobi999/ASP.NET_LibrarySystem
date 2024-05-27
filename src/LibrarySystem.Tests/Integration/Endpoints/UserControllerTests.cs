using System.Net.Http.Json;
using FluentAssertions;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class UserControllerTests
{
    [Fact]
    public async Task UserController_GetUsers_Returns200AndUsersAsync()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        var user3 = new User().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user1.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user2.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/auth/register", user3.ToRegisterUserDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync(string.Format("/api/user?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? throw new Exception("Failed to deserialize the response content.");
        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().Be(user2.ToDto());
    }
}
