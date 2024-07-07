using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class UserControllerTests
{
    [Fact]
    public async void UserController_GetUsers_Returns200AndUsersAsync()
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

        var content = await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? throw new NullReferenceException();
        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().Be(user2.ToDto());
    }

    [Fact]
    public async void UserController_GetUser_Returns200AndUserAsync()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync(string.Format("/api/user/{0}", user.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<UserDto>() ?? throw new NullReferenceException();
        content.Should().Be(user.ToDto());
    }

    [Fact]
    public async void UserController_UpdateUser_Returns200AndIsUpdatedAsync()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User"),
        ]);

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        // act & assert
        var updateDto = new UpdateUserDto
        {
            Username = "test",
            Email = "test@test.com",
        };

        var response = await client.PutAsJsonAsync(string.Format("/api/user/{0}", user.Id), updateDto); 
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        client.DefaultRequestHeaders.Remove("Authorization");

        var get = await client.GetAsync(string.Format("/api/user/{0}", user.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var newUser = await get.Content.ReadFromJsonAsync<UserDto>() ?? throw new NullReferenceException();
        newUser.Id.Should().Be(user.Id); 
        newUser.Username.Should().Be(updateDto.Username);
        newUser.Email.Should().Be(updateDto.Email);
    }

    [Fact]
    public async void UserController_DeleteUser_Returns200AndUserDoesntExistInTheDb()
    {
        //prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User"),
        ]);

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        // act & assert
        var response = await client.DeleteAsync(string.Format("/api/user/{0}", user.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/user/{0}", user.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
