
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LibrarySystem.Application.Core.Utilities;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Exceptions.HTTP;
using LibrarySystem.Tests.Integration.Helpers;

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

        var payload = JwtUtils.ParsePayload(token);
        payload.Count().Should().BeGreaterThan(0);
        payload.ElementAt(0).Type.Should().Be("AccountId");
        payload.ElementAt(0).Value.Should().Be(user.Id.ToString());
    }

    [Fact]
    public void Jwt_Parse_ValidationWorks()
    {
        // act & assert
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.Parse(""); });
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.Parse("Bearer"); });
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.Parse("Bearer "); });
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.Parse("TOKEN"); });
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
        JwtUtils.Parse($"Bearer {token}").Should().Be(token);
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
        JwtUtils.ParseFromPayload(token, "AccountId").Should().Be(user.Id.ToString());
        JwtUtils.ParseFromPayload(token, "ACCOUNTID").Should().Be(user.Id.ToString());
        JwtUtils.ParseFromPayload(token, "accountid").Should().Be(user.Id.ToString());

        JwtUtils.ParseFromPayload(token, "account_id").Should().BeNull();
    }
}
