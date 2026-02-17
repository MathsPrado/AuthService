using MyAuth.Domain.Dtos;

namespace MyAuth.Domain.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionDto>> GetAllAsync();
    Task<PermissionDto?> GetByIdAsync(int id);
    Task<PermissionDto> CreateAsync(string name, string? description = null);
    Task<IEnumerable<string>> GetRolesAsync(int permissionId);
}
