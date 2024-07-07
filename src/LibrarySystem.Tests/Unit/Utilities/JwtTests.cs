
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using LibrarySystem.Application.Services;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.BadRequest;
using LibrarySystem.Tests;
using LibrarySystem.Tests.Integration.Extensions;

namespace LibrarySystem.Tests.Unit.Utilities;

public class JwtTests
{
    [Fact]
    public void Jwt_Generate_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = "VERY_LONG_KEY_THAT_IS_SECURE_AND_STRONG";
        var jwt = new Jwt(issuer, key);

        // act & assert
        var token = jwt.Generate([]);
        token.Should().NotBeNull();

        var tokenData = new JwtSecurityTokenHandler().ReadJwtToken(token);

        tokenData.Issuer.Should().Be(issuer);
    }

    [Fact]
    public void Jwt_ParsePayload_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = "VERY_LONG_KEY_THAT_IS_SECURE_AND_STRONG";
        var jwt = new Jwt(issuer, key);

        var user = new User().WithFakeData();

        // act & assert
        var token = jwt.Generate([
            new Claim("AccountId", user.Id.ToString())
        ]);
        token.Should().NotBeNull();

        var payload = Jwt.ParsePayload(token);
        payload.Count().Should().BeGreaterThan(0);
        payload.ElementAt(0).Type.Should().Be("AccountId");
        payload.ElementAt(0).Value.Should().Be(user.Id.ToString());
    }

    [Fact]
    public void Jwt_Parse_ValidationWorks()
    {
        // act & assert
        Assert.Throws<BadRequestException>(() => { Jwt.Parse(""); });
        Assert.Throws<BadRequestException>(() => { Jwt.Parse("Bearer"); });
        Assert.Throws<BadRequestException>(() => { Jwt.Parse("Bearer "); });
        Assert.Throws<BadRequestException>(() => { Jwt.Parse("TOKEN"); });
    }

    [Fact]
    public void Jwt_Parse_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = "VERY_LONG_KEY_THAT_IS_SECURE_AND_STRONG";
        var jwt = new Jwt(issuer, key);

        var user = new User().WithFakeData();

        var token = jwt.Generate([
            new Claim("AccountId", user.Id.ToString())
        ]);
        token.Should().NotBeNull();

        // act & assert
        Jwt.Parse(string.Format("Bearer {0}", token)).Should().Be(token);
    }

    [Fact]
    public void Jwt_ParseFromPayload_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = "VERY_LONG_KEY_THAT_IS_SECURE_AND_STRONG";
        var jwt = new Jwt(issuer, key);

        var user = new User().WithFakeData();

        var token = jwt.Generate([
            new Claim("AccountId", user.Id.ToString())
        ]);
        token.Should().NotBeNull();

        // act & assert
        Jwt.ParseFromPayload(token, "AccountId").Should().Be(user.Id.ToString());
        Jwt.ParseFromPayload(token, "ACCOUNTID").Should().Be(user.Id.ToString());
        Jwt.ParseFromPayload(token, "accountid").Should().Be(user.Id.ToString());

        Jwt.ParseFromPayload(token, "account_id").Should().BeNull();
    }
}
