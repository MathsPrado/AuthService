using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MyAuth.IntegrationTests;

public class PermissionControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public PermissionControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsAllPermissions()
    {
        // Act
        var response = await _client.GetAsync("/api/permission");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var permissions = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(permissions.ValueKind == JsonValueKind.Array);
        Assert.True(permissions.GetArrayLength() >= 3); // At least ReadUsers, EditUsers, DeleteUsers from seed data
    }

    [Fact]
    public async Task Get_WithValidId_ReturnsPermission()
    {
        // Arrange - Using seeded ReadUsers permission with Id = 1
        var permissionId = 1;

        // Act
        var response = await _client.GetAsync($"/api/permission/{permissionId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var permission = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(permission.TryGetProperty("id", out var id));
        Assert.Equal(permissionId, id.GetInt32());
        Assert.True(permission.TryGetProperty("name", out var name));
        Assert.Equal("ReadUsers", name.GetString());
    }

    [Fact]
    public async Task Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidPermissionId = 999;

        // Act
        var response = await _client.GetAsync($"/api/permission/{invalidPermissionId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedPermission()
    {
        // Arrange
        var newPermission = new
        {
            Name = "CreateReports",
            Description = "Permission to create reports"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/permission", newPermission);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var permission = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(permission.TryGetProperty("id", out var id));
        Assert.True(id.GetInt32() > 0);
        Assert.True(permission.TryGetProperty("name", out var name));
        Assert.Equal("CreateReports", name.GetString());
    }

    [Fact]
    public async Task GetRoles_WithValidPermissionId_ReturnsRoles()
    {
        // Arrange - Using seeded ReadUsers permission (Id = 1) which is assigned to roles
        var permissionId = 1;

        // Act
        var response = await _client.GetAsync($"/api/permission/{permissionId}/roles");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var roles = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(roles.ValueKind == JsonValueKind.Array);
        Assert.True(roles.GetArrayLength() > 0); // ReadUsers is assigned to roles from seed data
    }

    [Fact]
    public async Task GetRoles_WithPermissionWithoutRoles_ReturnsEmptyArray()
    {
        // Arrange - Create a new permission without roles
        var newPermission = new
        {
            Name = "UnassignedPermission",
            Description = "Permission without roles"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/permission", newPermission);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var createdPermission = JsonSerializer.Deserialize<JsonElement>(createContent);
        var permissionId = createdPermission.GetProperty("id").GetInt32();

        // Act
        var response = await _client.GetAsync($"/api/permission/{permissionId}/roles");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var roles = JsonSerializer.Deserialize<JsonElement>(content);
        
        Assert.True(roles.ValueKind == JsonValueKind.Array);
        Assert.Equal(0, roles.GetArrayLength());
    }

    [Fact]
    public async Task Create_MultplePermissions_EachHasUniqueId()
    {
        // Arrange
        var permission1 = new
        {
            Name = "ViewDashboard",
            Description = "Permission to view dashboard"
        };

        var permission2 = new
        {
            Name = "EditDashboard",
            Description = "Permission to edit dashboard"
        };

        // Act
        var response1 = await _client.PostAsJsonAsync("/api/permission", permission1);
        var response2 = await _client.PostAsJsonAsync("/api/permission", permission2);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, response2.StatusCode);
        
        var content1 = await response1.Content.ReadAsStringAsync();
        var content2 = await response2.Content.ReadAsStringAsync();
        
        var perm1 = JsonSerializer.Deserialize<JsonElement>(content1);
        var perm2 = JsonSerializer.Deserialize<JsonElement>(content2);
        
        var id1 = perm1.GetProperty("id").GetInt32();
        var id2 = perm2.GetProperty("id").GetInt32();
        
        Assert.NotEqual(id1, id2);
    }
}
