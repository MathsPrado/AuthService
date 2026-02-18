using Xunit;
using Microsoft.EntityFrameworkCore;
using MyAuth.Domain.Entities;
using MyAuth.Data.Context;
using MyAuth.Data.Repositories;

namespace MyAuth.Tests.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        // Create in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ValidUser_Succeeds()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Password = "password" };

        // Act
        await _repository.AddAsync(user);

        // Assert
        var result = await _context.UsersSystem.FindAsync(1);
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_Returns()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Password = "password" };
        _context.UsersSystem.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentUser_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUsernameAsync_ExistingUsername_Returns()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Password = "password" };
        _context.UsersSystem.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUsernameAsync("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByUsernameAsync_NonExistentUsername_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByUsernameAsync("nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AssignRoleAsync_ValidAssignment_Succeeds()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Password = "password" };
        var role = new Role { Id = 1, Name = "User" };
        _context.UsersSystem.Add(user);
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        await _repository.AssignRoleAsync(1, 1);

        // Assert
        var assignment = await _context.UserRoles.FindAsync(1, 1);
        Assert.NotNull(assignment);
    }

    [Fact]
    public async Task GetRolesAsync_UserWithRoles_Returns()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Password = "password" };
        var role = new Role { Id = 1, Name = "Admin" };
        _context.UsersSystem.Add(user);
        _context.Roles.Add(role);
        _context.UserRoles.Add(new UserRole { UserId = 1, RoleId = 1, AssignedAt = DateTime.UtcNow });
        await _context.SaveChangesAsync();

        // Act
        var roles = await _repository.GetRolesAsync(1);

        // Assert
        Assert.Single(roles);
        Assert.Contains("Admin", roles);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
