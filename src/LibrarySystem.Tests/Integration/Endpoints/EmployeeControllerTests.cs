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
        var Employee1 = new Employee().WithFakeData();
        var Employee2 = new Employee().WithFakeData();
        var Employee3 = new Employee().WithFakeData();

        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create1 = await client.PostAsJsonAsync("/api/auth/Employee/register", Employee1.ToRegisterEmployeeDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/Employee/register", Employee2.ToRegisterEmployeeDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/auth/Employee/register", Employee3.ToRegisterEmployeeDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync(string.Format("/api/Employee?limit={0}&offset={1}", limit, offset));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<EmployeeDto>>() ?? throw new Exception("Failed to deserialize the response content.");
        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().Be(Employee2.ToDto());
    }
}
