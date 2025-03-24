using System.Security.Cryptography;
using LibrarySystem.Application.Core.Utilities;

namespace LibrarySystem.Tests.Unit.Utilities;

public class HasherTests
{
    [Fact]
    public void Hasher_Hash_Works()
    {
        // prepare
        var hasher = new Hasher(algorithm: HashAlgorithmName.SHA256);
        const string password = "TEST_PASSWORD_IN_PLAIN_TEXT";

        // act & assert
        var hashedPassword = hasher.Hash(password);

        password.Should().NotBe(hashedPassword);
    }

    [Fact]
    public void Hasher_Compare_Works()
    {
        // prepare
        var hasher = new Hasher(algorithm: HashAlgorithmName.SHA256);
        const string password1 = "TEST_PASSWORD_IN_PLAIN_TEXT";
        const string password2 = "ANOTHER_TEST_PASSWORD";

        var hashedPassword1 = hasher.Hash(password1);
        var hashedPassword2 = hasher.Hash(password2);

        // act & assert
        hasher.Compare(password1, hashedPassword1).Should().BeTrue();
        hasher.Compare(password2, hashedPassword2).Should().BeTrue();
        hasher.Compare(password1, hashedPassword2).Should().BeFalse();
        hasher.Compare(password2, hashedPassword1).Should().BeFalse();
    }
}