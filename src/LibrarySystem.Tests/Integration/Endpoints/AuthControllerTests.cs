using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Responses;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class AuthControllerTests
{
    [Fact]
    public async Task RegisterUser_Returns201AndUserIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();

        // act & assert
        var response = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // assert that the user is created
        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/user/{user.Id}");

        await using var context = app.GetDatabaseContext();
        context.Set<User>().Any(u => u.Id == user.Id).Should().BeTrue();
    }

    [Fact]
    public async Task LoginUser_Returns200AndJwtAndUser()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var loginDto = new LoginUserDto
        {
            Email = user.Email,
            Password = user.Password
        };

        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginUserResponseDto>() ?? throw new NullReferenceException();

        content.UserDto.Id.Should().Be(user.Id);
        content.Token.Should().NotBeNull();

        var payload = JwtUtils.ParsePayload(content.Token);

        payload.Count().Should().BeGreaterThan(2);
        payload.ElementAt(0).Type.Should().Be("UserId");
        payload.ElementAt(0).Value.Should().Be(user.Id.ToString());
        payload.ElementAt(1).Type.Should().Be("role");
        payload.ElementAt(1).Value.Should().Be("User");
    }

    [Fact]
    public async Task LoginUser_Returns400WhenUserLocked()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var loginDto = new LoginUserDto
        {
            Email = user.Email,
            Password = user.Email
        };

        for (var i = 0; i < User.AttemptsBeforeLock; i++)
        {
            var login = await client.PostAsJsonAsync("/api/auth/login", loginDto);
            login.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        // assert that the login lock is set up correctly
        await using var context = app.GetDatabaseContext();
        context.Set<User>().FirstOrDefault(u => u.Id == user.Id)!.LoginAttempts.Should().Be(User.AttemptsBeforeLock);

        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterEmployee_Returns201AndEmployeeIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var employee = EmployeeFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // act & assert
        var response = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/employee/{employee.Id}");

        // assert that the user is created
        await using var context = app.GetDatabaseContext();
        context.Set<Employee>().Any(e => e.Id == employee.Id).Should().BeTrue();
    }

    [Fact]
    public async Task LoginEmployee_Returns201AndJwtAndEmployee()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = EmployeeFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        // act & assert
        var loginDto = new LoginEmployeeDto
        {
            Email = employee.Email,
            Password = employee.Password
        };

        var response = await client.PostAsJsonAsync("/api/auth/employee/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginEmployeeResponseDto>() ?? throw new NullReferenceException();

        content.EmployeeDto.Id.Should().Be(employee.Id);
        content.Token.Should().NotBeNull();

        var payload = JwtUtils.ParsePayload(content.Token);

        payload.Count().Should().BeGreaterThan(2);
        payload.ElementAt(0).Type.Should().Be("EmployeeId");
        payload.ElementAt(0).Value.Should().Be(employee.Id.ToString());
        payload.ElementAt(1).Type.Should().Be("role");
        payload.ElementAt(1).Value.Should().Be("Employee");
    }

    [Fact]
    public async Task LoginEmployee_Returns400WhenEmployeeLocked()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var employee = EmployeeFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        // act & assert
        var loginDto = new LoginEmployeeDto
        {
            Email = employee.Email,
            Password = employee.Email
        };

        for (var i = 0; i < Employee.AttemptsBeforeLock; i++)
        {
            var login = await client.PostAsJsonAsync("/api/auth/employee/login", loginDto);
            login.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        // assert that the login lock is set up correctly
        await using var context = app.GetDatabaseContext();
        context.Set<Employee>().FirstOrDefault(e => e.Id == employee.Id)!.LoginAttempts.Should().Be(Employee.AttemptsBeforeLock);

        var response = await client.PostAsJsonAsync("/api/auth/employee/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}