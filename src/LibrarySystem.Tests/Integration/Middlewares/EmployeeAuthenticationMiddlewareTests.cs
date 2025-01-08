using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Middlewares;

public class EmployeeAuthenticationMiddlewareTests
{
    [Fact]
    public async void EmployeeAuthenticationMiddleware_Returns401WhenTryingToModifyDifferentEmployee()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee1 = EmployeeFactory.CreateWithFakeData();
        var employee2 = EmployeeFactory.CreateWithFakeData();

        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create1 = await client.PostAsJsonAsync("/api/auth/employee/register", employee1.ToRegisterEmployeeDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/auth/employee/register", employee2.ToRegisterEmployeeDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("EmployeeId", employee1.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        // act & assert
        var response1 = await client.DeleteAsync($"/api/employee/{employee2.Id}");
        response1.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var response2 = await client.DeleteAsync($"/api/employee/{employee1.Id}");
        response2.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}
