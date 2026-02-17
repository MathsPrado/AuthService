using MyAuth.Domain.Dtos;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var list = await _roleRepository.GetAllAsync();
        return list.Select(r => new RoleDto(r.Id, r.Name, r.Description));
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var r = await _roleRepository.GetByIdAsync(id);
        if (r == null) return null;
        return new RoleDto(r.Id, r.Name, r.Description);
    }

    public async Task<RoleDto> CreateAsync(string name, string? description = null, string? screen = null)
    {
        var role = new Role { Name = name, Description = description, Screen = screen };
        await _roleRepository.AddAsync(role);
        return new RoleDto(role.Id, role.Name, role.Description);
    }

    public async Task AssignPermissionAsync(int roleId, int permissionId)
    {
        await _roleRepository.AssignPermissionAsync(roleId, permissionId);
    }

    public async Task<IEnumerable<string>> GetPermissionsAsync(int roleId)
    {
        return await _roleRepository.GetPermissionsAsync(roleId);
    }
}
