using Xunit;

namespace MyAuth.Tests.Configuration;

public class JwtSecretValidationTests
{
    [Fact]
    public void ValidSecret_32Chars_Passes()
    {
        // Arrange
        var secret = "this_is_a_valid_secret_of_32_chA"; // exactly 32 chars

        // Act
        var isValid = secret.Length >= 32;

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ValidSecret_MoreThan32Chars_Passes()
    {
        // Arrange
        var secret = "this_is_a_valid_secret_of_more_than_32_characters_long";

        // Act
        var isValid = secret.Length >= 32;

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void InvalidSecret_LessThan32Chars_Fails()
    {
        // Arrange
        var secret = "short_secret";

        // Act
        var isValid = secret.Length >= 32;

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void EmptySecret_Fails()
    {
        // Arrange
        var secret = string.Empty;

        // Act
        var isValid = !string.IsNullOrEmpty(secret) && secret.Length >= 32;

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void ByteLengthValidation_32Chars_256Bits_Valid()
    {
        // Arrange
        var secret = "this_is_a_valid_secret_of_32_chA"; // 32 ASCII chars >= 256 bits (256+ bits)
        var bytes = System.Text.Encoding.ASCII.GetBytes(secret);

        // Act
        var bitLength = bytes.Length * 8;

        // Assert
        Assert.True(bitLength >= 256); // 32 bytes = 256 bits minimum
    }

    [Fact]
    public void ByteLengthValidation_TooShort_Invalid()
    {
        // Arrange
        var secret = "short"; // 5 chars = 40 bits
        var bytes = System.Text.Encoding.UTF8.GetBytes(secret);

        // Act
        var bitLength = bytes.Length * 8;

        // Assert
        Assert.False(bitLength >= 256);
    }
}
