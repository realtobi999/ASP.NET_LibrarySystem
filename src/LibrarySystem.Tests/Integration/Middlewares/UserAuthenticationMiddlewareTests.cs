﻿using System.Net.Http.Json;
using System.Security.Claims;
using LibrarySystem.Domain.Dtos.Reviews;
using LibrarySystem.Presentation;
using LibrarySystem.Tests.Integration.Factories;
using LibrarySystem.Tests.Integration.Helpers;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Middlewares;

public class UserAuthenticationMiddlewareTests
{
    [Fact]
    public async Task UserAuthenticationMiddleware_Returns401WhenTryingToModifyDifferentUser()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = UserFactory.CreateWithFakeData();
        var user2 = UserFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim("UserId", user1.Id.ToString()),
            new Claim(ClaimTypes.Role, "User")
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var create1 = await client.PostAsJsonAsync("/api/auth/register", user1.ToRegisterUserDto());
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user2.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        // act & assert
        var response1 = await client.DeleteAsync($"/api/user/{user2.Id}"); // try to delete the other user, authenticated as the first
        response1.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var response2 = await client.DeleteAsync($"/api/user/{user1.Id}");
        response2.StatusCode.Should().NotBe(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UserAuthenticationMiddleware_Returns401WhenTheIdIsWrongInTheBody()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = UserFactory.CreateWithFakeData();
        var token = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString())
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        // act & assert
        var response = await client.PostAsJsonAsync("/api/review", new CreateBookReviewDto
        {
            Id = Guid.Empty,
            BookId = Guid.Empty,
            UserId = Guid.NewGuid(), // insert bad id
            Rating = 0,
            Text = "test"
        });
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}