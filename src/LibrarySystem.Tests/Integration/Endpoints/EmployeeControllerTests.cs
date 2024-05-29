using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class EmployeeControllerTests
{
    [Fact]
    public async void EmployeeController_GetEmployees_Returns200AndLimitAndOffsetWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee1 = new Employee().WithFakeData();
        var employee2 = new Employee().WithFakeData();
        var employee3 = new Employee().WithFakeData();

        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/employee/register", employee1.ToRegisterEmployeeDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/employee/register", employee2.ToRegisterEmployeeDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/auth/employee/register", employee3.ToRegisterEmployeeDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync(string.Format("/api/employee?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<EmployeeDto>>() ?? throw new Exception("Failed to deserialize the response content.");
        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().Be(employee2.ToDto());
    }

    [Fact]
    public async void EmployeeController_GetEmployee_Returns200AndEmployee()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = new Employee().WithFakeData();

        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response1 = await client.GetAsync(string.Format("/api/employee/{0}", employee.Id));
        response1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response1.Content.ReadFromJsonAsync<EmployeeDto>() ?? throw new Exception("Failed to deserialize the response content.");
        content.Should().Be(employee.ToDto());

        var response2 = await client.GetAsync(string.Format("/api/employee/{0}", Guid.NewGuid()));
        response2.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
