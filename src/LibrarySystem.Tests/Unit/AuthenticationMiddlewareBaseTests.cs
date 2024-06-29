using System.Text;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Presentation.Middlewares;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Tests.Unit;

public class AuthenticationMiddlewareBaseTests : AuthenticationMiddlewareBase
{
    [Fact]
    public async void ExtractKeyFromRoute_WithValueInRoute_ReturnsCorrectValue()
    {
        // prepare
        var context = new DefaultHttpContext();
        context.Request.RouteValues["UserId"] = "123";

        // act & assert
        var result = await ExtractKeyFromRouteOrBodyAsync(context, "UserId");

        Assert.Equal("123", result);
    }

    [Fact]
    public async void ExtractKeyFromRoute_WithValueInBody_ReturnsCorrectValue()
    {
        // prepare
        var requestBody = "{\"UserId\": \"456\"}";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
        var context = new DefaultHttpContext();
        context.Request.Body = stream;
        context.Request.ContentType = "application/json";

        // act & assert
        var result = await ExtractKeyFromRouteOrBodyAsync(context, "UserId");

        Assert.Equal("456", result);
    }

    [Fact]
    public async void ExtractKeyFromRouteOrBody_KeyNotFound_ThrowsBadRequestException()
    {
        // prepare
        var requestBody = "{\"Test\": \"456\"}";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
        var context = new DefaultHttpContext();
        context.Request.Body = stream;
        context.Request.ContentType = "application/json";

        // act & assert 
        await Assert.ThrowsAsync<BadRequestException>(async () =>
        {
            await ExtractKeyFromRouteOrBodyAsync(context, "UserId");
        });
    }
}
