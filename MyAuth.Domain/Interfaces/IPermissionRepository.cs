using MyAuth.Domain.Entities;

namespace MyAuth.Domain.Interfaces;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id);
    Task<IEnumerable<Permission>> GetAllAsync();
    Task AddAsync(Permission permission);
    Task<IEnumerable<string>> GetRolesAsync(int permissionId);
}
