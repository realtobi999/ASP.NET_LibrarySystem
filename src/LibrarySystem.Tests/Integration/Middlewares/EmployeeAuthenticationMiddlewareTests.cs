using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class EmployeeAuthenticationMiddlewareTests
{
    [Fact]
    public async void EmployeeAuthenticationMiddleware_Returns401WhenTryingToModifyDifferentEmployee()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee1 = new Employee().WithFakeData();
        var employee2 = new Employee().WithFakeData();

        var token1 = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/auth/employee/register", employee1.ToRegisterEmployeeDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var create2 = await client.PostAsJsonAsync("/api/auth/employee/register", employee2.ToRegisterEmployeeDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        var token2 = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("EmployeeId", employee1.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        // act & assert
        var response1 = await client.DeleteAsync(string.Format("/api/employee/{0}", employee2.Id));
        response1.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var response2 = await client.DeleteAsync(string.Format("/api/employee/{0}", employee1.Id));
        response2.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
