﻿using System.Net.Http.Json;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Domain.Dtos.Messages;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Tests.Integration.Extensions;
using LibrarySystem.Tests.Integration.Server;

namespace LibrarySystem.Tests.Integration.Endpoints;

public class BookReviewControllerTests
{
    [Fact]
    public async void BookReviewController_CreateBookReview_Returns201()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var book = new Book().WithFakeData();
        var user = new User().WithFakeData();
        var review = new BookReview().WithFakeData(book, user);
        var token1 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "Employee")
        ]);

        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token1));

        var create1 = await client.PostAsJsonAsync("/api/book", book.ToCreateBookDto([], []));
        create1.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("/api/auth/register", user.ToRegisterUserDto());
        create2.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

        var token2 = JwtTestExtensions.Create().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token2));

        // act & assert
        var response = await client.PostAsJsonAsync("/api/book/review", review.ToCreateBookReviewDto());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }
}