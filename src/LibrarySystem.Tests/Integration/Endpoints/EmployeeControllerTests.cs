using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Employees;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class EmployeeControllerTests
{
    [Fact]
    public async void GetEmployees_Returns200AndLimitAndOffsetWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee1 = EmployeeFactory.CreateWithFakeData();
        var employee2 = EmployeeFactory.CreateWithFakeData();
        var employee3 = EmployeeFactory.CreateWithFakeData();

        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create1 = await client.PostAsJsonAsync("/api/auth/employee/register", employee1.ToRegisterEmployeeDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/employee/register", employee2.ToRegisterEmployeeDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/auth/employee/register", employee3.ToRegisterEmployeeDto());
        create3.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync($"/api/employee?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<EmployeeDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Should().Be(employee2.ToDto());
    }

    [Fact]
    public async void GetEmployee_Returns200AndEmployee()
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

        // act & assert
        var response1 = await client.GetAsync($"/api/employee/{employee.Id}");
        response1.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await response1.Content.ReadFromJsonAsync<EmployeeDto>() ?? throw new NullReferenceException();

        content.Should().Be(employee.ToDto());

        var response2 = await client.GetAsync($"/api/employee/{Guid.NewGuid()}");
        response2.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async void UpdateEmployee_Returns204AndEmployeeIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = EmployeeFactory.CreateWithFakeData();

        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("EmployeeId", employee.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        // act & assert
        var updateDto = new UpdateEmployeeDto
        {
            Name = "test",
            Email = "test@test.com"
        };

        var response = await client.PutAsJsonAsync($"api/employee/{employee.Id}", updateDto);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get = await client.GetAsync($"/api/employee/{employee.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<EmployeeDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(employee.Id);
        content.Name.Should().Be(updateDto.Name);
        content.Email.Should().Be(updateDto.Email);
    }

    [Fact]
    public async void UpdateEmployee_Returns204AndEmployeeIsDeleted()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = EmployeeFactory.CreateWithFakeData();

        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("EmployeeId", employee.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        // act & assert
        var response = await client.DeleteAsync($"/api/employee/{employee.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get = await client.GetAsync($"/api/employee/{employee.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async void UploadPhotos_Returns204AndIsUploaded()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var employee = EmployeeFactory.CreateWithFakeData();
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Admin")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token1}");

        var create = await client.PostAsJsonAsync("/api/auth/employee/register", employee.ToRegisterEmployeeDto());
        create.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim("EmployeeId", employee.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token2}");

        // create photo files and then assign them
        var photo1 = new ByteArrayContent([1, 2, 3, 4]);
        photo1.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        var photo2 = new ByteArrayContent([5, 6, 7, 8]);
        photo2.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        var formData = new MultipartFormDataContent
        {
            { photo1, "file", "photo1.jpg" },
        };

        // act & assert
        var response = await client.PutAsync($"/api/employee/{employee.Id}/photo", formData);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var get = await client.GetAsync($"/api/employee/{employee.Id}");
        get.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<EmployeeDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(employee.Id);
        content.Picture.Should().NotBeNull();
        content.Picture!.FileName.Should().Be("photo1.jpg");
    }
}
