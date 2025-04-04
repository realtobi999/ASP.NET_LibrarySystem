﻿using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Users;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class UserControllerTests
{
    [Fact]
    public async Task GetUsers_Returns200AndCorrectValues()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = UserFactory.CreateWithFakeData();
        var user2 = UserFactory.CreateWithFakeData();
        var user3 = UserFactory.CreateWithFakeData();

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user1.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user2.ToRegisterUserDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("/api/auth/register", user3.ToRegisterUserDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        const int limit = 2;
        const int offset = 1;

        var response = await client.GetAsync($"/api/user?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(user2.Id);
    }

    [Fact]
    public async Task GetUser_Returns200AndCorrectValue()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"/api/user/{user.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<UserDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateUser_Returns204AndUserIsUpdated()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User")
        ]);

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // act & assert
        var updateDto = new UpdateUserDto
        {
            Username = "test",
            Email = "test@test.com"
        };

        var response = await client.PutAsJsonAsync($"/api/user/{user.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the user is updated
        await using var context = app.GetDatabaseContext();
        var updatedUser = context.Set<User>().FirstOrDefault(u => u.Id == user.Id) ?? throw new NullReferenceException();

        updatedUser.Username.Should().Be(updateDto.Username);
        updatedUser.Email.Should().Be(updateDto.Email);
    }

    [Fact]
    public async Task DeleteUser_Returns204AndUserIsDeleted()
    {
        //prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User")
        ]);

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // act & assert
        var response = await client.DeleteAsync($"/api/user/{user.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // assert that the user is deleted
        await using var context = app.GetDatabaseContext();
        context.Set<User>().Any(u => u.Id == user.Id).Should().BeFalse();
    }

    [Fact]
    public async Task UploadPhotos_Returns204AndPhotoIsUploaded()
    {
        // prepare
        var app = new WebAppFactory<Program>();
        var client = app.CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create.StatusCode.Should().Be(HttpStatusCode.Created);

        var photo1 = new ByteArrayContent([1, 2, 3, 4]);
        photo1.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        var photo2 = new ByteArrayContent([5, 6, 7, 8]);
        photo2.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

        var formData = new MultipartFormDataContent
        {
            { photo1, "file", "photo1.jpg" }
        };

        // act & assert
        var response = await client.PutAsync($"/api/user/{user.Id}/photo", formData);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);


        // assert that the user profile picture is uploaded
        await using var context = app.GetDatabaseContext();
        var updatedUser = context.Set<User>().Include(u => u.ProfilePicture).FirstOrDefault(u => u.Id == user.Id) ??
                          throw new NullReferenceException();

        updatedUser.ProfilePicture.Should().NotBeNull();
        updatedUser.ProfilePicture!.FileName.Should().Be("photo1.jpg");
    }
}