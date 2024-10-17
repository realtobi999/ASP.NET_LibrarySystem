using LibrarySystem.Application.Core.Utilities;

namespace LibrarySystem.Tests.Unit.Utilities;

public class HasherTests
{
    [Fact]
    public void Hasher_Hash_Works()
    {
        // prepare
        var hasher = new Hasher();

        var password = "TEST_PASSWORD_IN_PLAIN_TEXT";

        // act & assert
        var hashedPassword = hasher.Hash(password);

        password.Should().NotBe(hashedPassword);
    }

    [Fact]
    public void Hasher_Compare_Works()
    {
        // prepare
        var hasher = new Hasher();

        var password1 = "TEST_PASSWORD_IN_PLAIN_TEXT";
        var password2 = "ANOTHER_TEST_PASSWORD";

        var hashedPassword1 = hasher.Hash(password1);
        var hashedPassword2 = hasher.Hash(password2);

        // act & assert
        hasher.Compare(password1, hashedPassword1).Should().BeTrue();
        hasher.Compare(password2, hashedPassword2).Should().BeTrue();
        hasher.Compare(password1, hashedPassword2).Should().BeFalse();
        hasher.Compare(password2, hashedPassword1).Should().BeFalse();
    }
}
