
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Tests.Integration.Extensions;

namespace LibrarySystem.Tests;

public class JwtTokenTests
{
    [Fact]
    public void JwtToken_Generate_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = "VERY_LONG_KEY_THAT_IS_SECURE_AND_STRONG";
        var jwt = new JwtToken(issuer, key);

        // act & assert
        var token = jwt.Generate([]);
        token.Should().NotBeNull();

        var tokenData = new JwtSecurityTokenHandler().ReadJwtToken(token);

        tokenData.Issuer.Should().Be(issuer);
    }

    [Fact]
    public void JwtToken_ParsePayload_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = "VERY_LONG_KEY_THAT_IS_SECURE_AND_STRONG";
        var jwt = new JwtToken(issuer, key);
        
        var user = new User().WithFakeData();

        // act & assert
        var token = jwt.Generate([
            new Claim("AccountId", user.Id.ToString())
        ]);
        token.Should().NotBeNull();

        var payload = JwtToken.ParsePayload(token); 
        payload.Count().Should().BeGreaterThan(0);
        payload.ElementAt(0).Type.Should().Be("AccountId");
        payload.ElementAt(0).Value.Should().Be(user.Id.ToString());
    }

    [Fact]
    public void JwtToken_Parse_ValidationWorks()
    {
        // act & assert
        Assert.Throws<BadRequestException>(() => { JwtToken.Parse("");});
        Assert.Throws<BadRequestException>(() => { JwtToken.Parse("Bearer");});
        Assert.Throws<BadRequestException>(() => { JwtToken.Parse("Bearer ");});
        Assert.Throws<BadRequestException>(() => { JwtToken.Parse("TOKEN");});
    }

    [Fact]
    public void JwtToken_Parse_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = "VERY_LONG_KEY_THAT_IS_SECURE_AND_STRONG";
        var jwt = new JwtToken(issuer, key);
        
        var user = new User().WithFakeData();

        var token = jwt.Generate([
            new Claim("AccountId", user.Id.ToString())
        ]);
        token.Should().NotBeNull();

        // act & assert
        JwtToken.Parse(string.Format("Bearer {0}", token)).Should().Be(token);
    }
}
