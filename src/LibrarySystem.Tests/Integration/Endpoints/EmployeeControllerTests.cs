using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Employees;
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

    [Fact]
    public async void EmployeeController_UpdateEmployee_Returns200AndEmployeeIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = new Employee().WithFakeData();

        var token1 = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        var token2 = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("EmployeeId", employee.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        // act & assert
        var updateDto = new UpdateEmployeeDto
        {
            Name = "test",
            Email = "test@test.com"
        };

        var response = await client.PutAsJsonAsync(string.Format("api/employee/{0}", employee.Id), updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/employee/{0}", employee.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var updatedEmployee = await get.Content.ReadFromJsonAsync<EmployeeDto>() ?? throw new Exception("Failed to deserialize the response content.");
        updatedEmployee.Id.Should().Be(employee.Id);
        updatedEmployee.Name.Should().Be(updateDto.Name);
        updatedEmployee.Email.Should().Be(updateDto.Email);
    }

    [Fact]
    public async void EmployeeController_UpdateEmployee_Returns200AndEmployeeIsDeleted()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = new Employee().WithFakeData();

        var token1 = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        var token2 = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("EmployeeId", employee.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        // act & assert
        var response = await client.DeleteAsync(string.Format("/api/employee/{0}", employee.Id));
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var get = await client.GetAsync(string.Format("/api/employee/{0}", employee.Id));
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

}
