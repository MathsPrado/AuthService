using MyAuth.Domain.Entities;

namespace MyAuth.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task AddAsync(Role role);
    Task AssignPermissionAsync(int roleId, int permissionId);
    Task<IEnumerable<string>> GetPermissionsAsync(int roleId);
    Task AssignPageAsync(int roleId, int pageId);
    Task RemovePageAsync(int roleId, int pageId);
}
