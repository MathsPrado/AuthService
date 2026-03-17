namespace MyAuth.Domain.Entities;

public class RolePage
{
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public int PageId { get; set; }
    public Page Page { get; set; } = null!;

    public DateTime AssignedAt { get; set; }
}
