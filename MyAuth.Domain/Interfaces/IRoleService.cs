using MyAuth.Domain.Dtos;

namespace MyAuth.Domain.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto> CreateAsync(string name, string? description = null);
    Task AssignPermissionAsync(int roleId, int permissionId);
    Task<IEnumerable<string>> GetPermissionsAsync(int roleId);
    Task AssignPageAsync(int roleId, int pageId);
    Task RemovePageAsync(int roleId, int pageId);
}
