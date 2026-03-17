namespace MyAuth.Domain.Entities;

public class Page
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;       // ex: "Dashboard", "UserManagement"
    public string? Description { get; set; }
    public string? Route { get; set; }                      // ex: "/admin/users"
}
