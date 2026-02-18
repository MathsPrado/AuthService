using Xunit;
using Microsoft.EntityFrameworkCore;
using MyAuth.Domain.Entities;
using MyAuth.Data.Context;
using MyAuth.Data.Repositories;

namespace MyAuth.Tests.Repositories;

public class RoleRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly RoleRepository _repository;

    public RoleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        _repository = new RoleRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ValidRole_Succeeds()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "Admin", Description = "Admin role" };

        // Act
        await _repository.AddAsync(role);

        // Assert
        var result = await _context.Roles.FindAsync(1);
        Assert.NotNull(result);
        Assert.Equal("Admin", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingRole_Returns()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "Admin", Description = "Admin role" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.Name);
    }

    [Fact]
    public async Task GetAllAsync_MultipleRoles_Returns()
    {
        // Arrange
        _context.Roles.AddRange(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_ExistingRole_Succeeds()
    {
        // Arrange
        var role = new Role { Id = 1, Name = "Admin", Description = "Admin role" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        role.Description = "Updated description";
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Roles.FindAsync(1);
        Assert.Equal("Updated description", result.Description);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
