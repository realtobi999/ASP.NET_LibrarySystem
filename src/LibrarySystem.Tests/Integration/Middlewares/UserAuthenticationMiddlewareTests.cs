using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Middlewares;

public class UserAuthenticationMiddlewareTests
{
    [Fact]
    public async void UserAuthenticationMiddleware_Returns401WhenTryingToModifyDifferentAccount()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        // user1 jwt token
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim("UserId", user1.Id.ToString()),
            new Claim(ClaimTypes.Role, "User"),
        ]);

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user1.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/auth/register", user2.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // authenticate as the user1
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        // act & assert
        var response1 = await client.DeleteAsync(string.Format("/api/user/{0}", user2.Id)); // try to delete the other user, authenticated as the first
        response1.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var response2 = await client.DeleteAsync(string.Format("/api/user/{0}", user1.Id));
        response2.StatusCode.Should().NotBe(System.Net.HttpStatusCode.Unauthorized);
    }
}