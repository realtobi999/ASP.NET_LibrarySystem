using System.Text;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Presentation.Middlewares;
using Microsoft.AspNetCore.Http;

namespace LibrarySystem.Tests.Unit.Utilities;

public class AuthenticationMiddlewareBaseTests : AuthenticationMiddlewareBase
{
    [Fact]
    public async Task ExtractKeyFromRoute_WithValueInRoute_ReturnsCorrectValue()
    {
        // prepare
        var context = new DefaultHttpContext();
        context.Request.RouteValues["UserId"] = "123";

        // act & assert
        var result = await ExtractKeyFromRouteOrBodyAsync(context, "UserId");

        Assert.Equal("123", result);
    }

    [Fact]
    public async Task ExtractKeyFromRoute_WithValueInBody_ReturnsCorrectValue()
    {
        // prepare
        const string body = "{\"UserId\": \"456\"}";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(body));
        var context = new DefaultHttpContext
        {
            Request =
            {
                Body = stream,
                ContentType = "application/json"
            }
        };

        // act & assert
        var result = await ExtractKeyFromRouteOrBodyAsync(context, "UserId");

        Assert.Equal("456", result);
    }

    [Fact]
    public async Task ExtractKeyFromRouteOrBody_KeyNotFound_ThrowsBadRequestException()
    {
        // prepare
        const string body = "{\"Test\": \"456\"}";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(body));
        var context = new DefaultHttpContext
        {
            Request =
            {
                Body = stream,
                ContentType = "application/json"
            }
        };

        // act & assert 
        await Assert.ThrowsAsync<BadRequest400Exception>(async () => { await ExtractKeyFromRouteOrBodyAsync(context, "UserId"); });
    }
}