using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;
using MyAuth.Data.Services;

namespace MyAuth.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configMock = new Mock<IConfiguration>();
        _configMock.Setup(c => c["JwtSettings:Secret"])
            .Returns("this_is_a_valid_secret_of_32_characters_long");
        _authService = new AuthService(_userRepositoryMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ValidUser_ReturnsToken()
    {
        // Arrange
        var username = "newuser";
        var password = "password123";
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync((User)null!);
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<string> { "User" });
        _userRepositoryMock.Setup(r => r.GetPermissionsAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<string> { "ReadUsers" });

        // Act
        var token = await _authService.RegisterAsync(username, password);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_UserExists_ReturnsNull()
    {
        // Arrange
        var username = "admin";
        var existingUser = new User { Id = 1, Username = username, Password = "hash" };
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _authService.RegisterAsync(username, "password123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var username = "admin";
        var password = "admin";
        var user = new User { Id = 1, Username = username, Password = password };
        
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(1))
            .ReturnsAsync(new List<string> { "Admin" });
        _userRepositoryMock.Setup(r => r.GetPermissionsAsync(1))
            .ReturnsAsync(new List<string> { "FullAccess" });

        // Act
        var token = await _authService.LoginAsync(username, password);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var username = "admin";
        var user = new User { Id = 1, Username = username, Password = "admin" };
        
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        // Act
        var token = await _authService.LoginAsync(username, "wrongpassword");

        // Assert
        Assert.Null(token);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsNull()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null!);

        // Act
        var token = await _authService.LoginAsync("nonexistent", "password");

        // Assert
        Assert.Null(token);
    }
}
