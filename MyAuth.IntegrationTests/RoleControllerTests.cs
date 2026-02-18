using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyAuth.IntegrationTests;

public class RoleControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public RoleControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsAllRoles()
    {
        // Act
        var response = await _client.GetAsync("/api/role");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var roles = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(roles.ValueKind == JsonValueKind.Array);
        Assert.True(roles.GetArrayLength() >= 2); // At least Admin and User from seed data
    }

    [Fact]
    public async Task Get_WithValidId_ReturnsRole()
    {
        // Arrange - Using seeded Admin role with Id = 1
        var roleId = 1;

        // Act
        var response = await _client.GetAsync($"/api/role/{roleId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var role = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(role.TryGetProperty("id", out var id));
        Assert.Equal(roleId, id.GetInt32());
        Assert.True(role.TryGetProperty("name", out var name));
        Assert.Equal("Admin", name.GetString());
    }

    [Fact]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidRoleId = 999;

        // Act
        var response = await _client.GetAsync($"/api/role/{invalidRoleId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedRole()
    {
        // Arrange
        var newRole = new
        {
            Name = "Manager",
            Description = "Manager role"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/role", newRole);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var role = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(role.TryGetProperty("id", out var id));
        Assert.True(id.GetInt32() > 0);
        Assert.True(role.TryGetProperty("name", out var name));
        Assert.Equal("Manager", name.GetString());
    }

    [Fact]
    public async Task AssignPermission_WithValidIds_ReturnsNoContent()
    {
        // Arrange - Create a new role first
        var newRole = new
        {
            Name = "TestRole",
            Description = "Test role for permissions"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/role", newRole);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdRole = JsonSerializer.Deserialize<JsonElement>(createContent);
        var roleId = createdRole.GetProperty("id").GetInt32();

        // Use seeded permission with Id = 1 (ReadUsers)
        var permissionId = 1;

        // Act
        var response = await _client.PostAsJsonAsync($"/api/role/{roleId}/permissions", permissionId);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GetPermissions_WithValidRoleId_ReturnsPermissions()
    {
        // Arrange - Using seeded Admin role (Id = 1) which has permissions
        var roleId = 1;

        // Act
        var response = await _client.GetAsync($"/api/role/{roleId}/permissions");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var permissions = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(permissions.ValueKind == JsonValueKind.Array);
        Assert.True(permissions.GetArrayLength() > 0); // Admin has permissions from seed data
    }

    [Fact]
    public async Task GetPermissions_WithRoleWithoutPermissions_ReturnsEmptyArray()
    {
        // Arrange - Create a new role without permissions
        var newRole = new
        {
            Name = "EmptyRole",
            Description = "Role without permissions"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/role", newRole);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdRole = JsonSerializer.Deserialize<JsonElement>(createContent);
        var roleId = createdRole.GetProperty("id").GetInt32();

        // Act
        var response = await _client.GetAsync($"/api/role/{roleId}/permissions");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var permissions = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(permissions.ValueKind == JsonValueKind.Array);
        Assert.Equal(0, permissions.GetArrayLength());
    }
}
