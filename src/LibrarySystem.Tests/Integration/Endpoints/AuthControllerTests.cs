using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos;
using LibrarySystem.Domain.Dtos.Responses;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

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
        header.Should().Equal(string.Format("/api/user/{0}", user.Id));
    }

    [Fact]
    public async void AuthController_LoginUser_Returns200AndJwtTokenAndUser()
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

        var content = await response.Content.ReadFromJsonAsync<LoginUserResponseDto>() ?? throw new Exception("Failed to deserialize the response content.");

        content.UserDto!.Id.Should().Be(user.Id);
        content.Token.Should().NotBeNull();

        var tokenPayload = JwtToken.ParsePayload(content.Token!);
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
        var Employee = new Employee().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        // act & assert
        var response = await client.PostAsJsonAsync("/api/auth/Employee/register", Employee.ToRegisterEmployeeDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/api/Employee/{0}", Employee.Id));
    }

    [Fact]
    public async void AuthController_LoginEmployee_Returns201AndJwtTokenAndEmployee()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var Employee = new Employee().WithFakeData();
        var token = JwtTokenTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));

        var create = await client.PostAsJsonAsync("/api/auth/Employee/register", Employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        // act & assert
        var loginDto = new LoginEmployeeDto
        {
            Email = Employee.Email,
            Password = Employee.Password,
        };

        var response = await client.PostAsJsonAsync("/api/auth/Employee/login", loginDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginEmployeeResponseDto>() ?? throw new Exception("Failed to deserialize the response content.");
        
        content.EmployeeDto!.Id.Should().Be(Employee.Id);
        content.Token.Should().NotBeNull();

        var tokenPayload = JwtToken.ParsePayload(content.Token!);
        tokenPayload.Count().Should().BeGreaterThan(2);
        tokenPayload.ElementAt(0).Type.Should().Be("EmployeeId");
        tokenPayload.ElementAt(0).Value.Should().Be(Employee.Id.ToString());
        tokenPayload.ElementAt(1).Type.Should().Be("role");
        tokenPayload.ElementAt(1).Value.Should().Be("Employee");
    }

}
