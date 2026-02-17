namespace MyAuth.Domain.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;    // e.g. "ReadUsers", "EditProfile"
    public string? Description { get; set; }

    // relacionamentos
    // relationships omitted here to keep entity lean; use repository methods for joins
}

