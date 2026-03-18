using MyAuth.Domain.Entities;

namespace MyAuth.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();

    // adiciona um novo usuário ao banco de dados
    Task AddAsync(User user);

    // Métodos auxiliares para gerenciar associações
    Task AssignRoleAsync(int userId, int roleId);
    Task RemoveRoleAsync(int userId, int roleId);
    Task AssignPermissionAsync(int userId, int permissionId);
    Task RemovePermissionAsync(int userId, int permissionId);
    Task<IEnumerable<string>> GetRolesAsync(int userId);
    Task<IEnumerable<string>> GetPermissionsAsync(int userId);
}