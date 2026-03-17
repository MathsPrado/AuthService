namespace MyAuth.Domain.Entities;

public class Page
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Route { get; set; }

    public ICollection<RolePage> RolePages { get; set; } = new List<RolePage>();
}
