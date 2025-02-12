using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Presentation;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Domain.Dtos.Responses;
using LibrarySystem.Tests.Integration.Server;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class AuthControllerTests
{
    [Fact]
    public async void RegisterUser_Returns201AndUserIsCreated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();

        // act & assert
        var response = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // assert that the user is created
        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/user/{user.Id}");

        using var context = app.GetDatabaseContext();
        context.Set<User>().Any(u => u.Id == user.Id).Should().BeTrue();
    }

    [Fact]
    public async void LoginUser_Returns200AndJwtAndUser()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();

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
    public async void LoginUser_Returns400WhenUserLocked()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var loginDto = new LoginUserDto
        {
            Email = user.Email,
            Password = user.Email
        };

        for (int i = 0; i < User.AttemptsBeforeLock; i++)
        {
            var login = await client.PostAsJsonAsync("/api/auth/login", loginDto);
            login.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        // assert that the login lock is setup correctly
        using var context = app.GetDatabaseContext();
        context.Set<User>().FirstOrDefault(u => u.Id == user.Id)!.LoginAttempts.Should().Be(User.AttemptsBeforeLock);

        var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void RegisterEmployee_Returns201AndEmployeeIsCreated()
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
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal($"/api/employee/{employee.Id}");

        // assert that the user is created
        using var context = app.GetDatabaseContext();
        context.Set<Employee>().Any(e => e.Id == employee.Id).Should().BeTrue();
    }

    [Fact]
    public async void LoginEmployee_Returns201AndJwtAndEmployee()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = EmployeeFactory.CreateWithFakeData();
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

    [Fact]
    public async void LoginEmployee_Returns400WhenEmployeeLocked()
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
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        // act & assert
        var loginDto = new LoginEmployeeDto
        {
            Email = employee.Email,
            Password = employee.Email,
        };

        for (int i = 0; i < Employee.AttemptsBeforeLock; i++)
        {
            var login = await client.PostAsJsonAsync("/api/auth/employee/login", loginDto);
            login.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        // assert that the login lock is setup correctly
        using var context = app.GetDatabaseContext();
        context.Set<Employee>().FirstOrDefault(e => e.Id == employee.Id)!.LoginAttempts.Should().Be(Employee.AttemptsBeforeLock);

        var response = await client.PostAsJsonAsync("/api/auth/employee/login", loginDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
