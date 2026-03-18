using MyAuth.Domain.Dtos;

namespace MyAuth.Domain.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task AssignRoleAsync(int userId, int roleId);
    Task RemoveRoleAsync(int userId, int roleId);
    Task AssignPermissionAsync(int userId, int permissionId);
    Task RemovePermissionAsync(int userId, int permissionId);
}
