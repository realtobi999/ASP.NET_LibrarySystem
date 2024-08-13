using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Responses;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Presentation;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class AuthControllerTests
{
    [Fact]
    public async void AuthController_RegisterUser_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        // act & assert
        var response = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/user/{user.Id}");
    }

    [Fact]
    public async void AuthController_LoginUser_Returns200AndJwtAndUser()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var loginDto = new LoginUserDto
        {
            Email = user.Email,
            Password = user.Password
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginUserResponseDto>() ?? throw new NullReferenceException();

        content.UserDto!.Id.Should().Be(user.Id);
        content.Token.Should().NotBeNull();

        var tokenPayload = JwtUtils.ParsePayload(content.Token!);
        tokenPayload.Count().Should().BeGreaterThan(2);
        tokenPayload.ElementAt(0).Type.Should().Be("UserId");
        tokenPayload.ElementAt(0).Value.Should().Be(user.Id.ToString());
        tokenPayload.ElementAt(1).Type.Should().Be("role");
        tokenPayload.ElementAt(1).Value.Should().Be("User");
    }

    [Fact]
    public async void AuthController_LoginUser_Returns401()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var loginDto = new LoginUserDto
        {
            Email = user.Email,
            Password = user.Password + "BLEEH"
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]    
    public async void AuthController_RegisterEmployee_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = new Employee().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // act & assert
        var response = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/employee/{employee.Id}");
    }

    [Fact]
    public async void AuthController_LoginEmployee_Returns201AndJwtAndEmployee()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = new Employee().WithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        // act & assert
        var loginDto = new LoginEmployeeDto
        {
            Email = employee.Email,
            Password = employee.Password,
        };

        var response = await client.PostAsJsonAsync("/api/auth/employee/login", loginDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginEmployeeResponseDto>() ?? throw new NullReferenceException();
        
        content.EmployeeDto!.Id.Should().Be(employee.Id);
        content.Token.Should().NotBeNull();

        var tokenPayload = JwtUtils.ParsePayload(content.Token!);
        tokenPayload.Count().Should().BeGreaterThan(2);
        tokenPayload.ElementAt(0).Type.Should().Be("EmployeeId");
        tokenPayload.ElementAt(0).Value.Should().Be(employee.Id.ToString());
        tokenPayload.ElementAt(1).Type.Should().Be("role");
        tokenPayload.ElementAt(1).Value.Should().Be("Employee");
    }

}
