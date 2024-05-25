using System.Net.Http.Json;
using FluentAssertions;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class AuthControllerTests
{
    [Fact]
    private async Task AuthController_RegisterUser_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        // act & assert
        var response = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");

        header.Should().Equal(string.Format("/api/user/{0}", user.Id));
    }
}
