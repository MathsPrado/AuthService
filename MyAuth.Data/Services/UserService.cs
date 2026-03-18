using MyAuth.Domain.Dtos;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(ToDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        return ToDto(user);
    }

    public async Task AssignRoleAsync(int userId, int roleId)
    {
        await _userRepository.AssignRoleAsync(userId, roleId);
    }

    public async Task RemoveRoleAsync(int userId, int roleId)
    {
        await _userRepository.RemoveRoleAsync(userId, roleId);
    }

    public async Task AssignPermissionAsync(int userId, int permissionId)
    {
        await _userRepository.AssignPermissionAsync(userId, permissionId);
    }

    public async Task RemovePermissionAsync(int userId, int permissionId)
    {
        await _userRepository.RemovePermissionAsync(userId, permissionId);
    }

    private static UserDto ToDto(User u)
    {
        var roles = u.UserRoles.Select(ur =>
        {
            var pages = ur.Role.RolePages
                .Select(rp => new PageDto(rp.Page.Id, rp.Page.Name, rp.Page.Description, rp.Page.Route))
                .ToList();

            return new RoleDto(ur.Role.Id, ur.Role.Name, ur.Role.Description, pages);
        }).ToList();

        var directPermissions = u.UserPermissions
            .Select(up => new PermissionDto(up.Permission.Id, up.Permission.Name, up.Permission.Description))
            .ToList();

        return new UserDto(u.Id, u.Username, roles, directPermissions);
    }
}
