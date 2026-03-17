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
        return list.Select(ToDto);
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var r = await _roleRepository.GetByIdAsync(id);
        if (r == null) return null;
        return ToDto(r);
    }

    public async Task<RoleDto> CreateAsync(string name, string? description = null)
    {
        var role = new Role { Name = name, Description = description };
        await _roleRepository.AddAsync(role);
        return ToDto(role);
    }

    public async Task AssignPermissionAsync(int roleId, int permissionId)
    {
        await _roleRepository.AssignPermissionAsync(roleId, permissionId);
    }

    public async Task<IEnumerable<string>> GetPermissionsAsync(int roleId)
    {
        return await _roleRepository.GetPermissionsAsync(roleId);
    }

    public async Task AssignPageAsync(int roleId, int pageId)
    {
        await _roleRepository.AssignPageAsync(roleId, pageId);
    }

    public async Task RemovePageAsync(int roleId, int pageId)
    {
        await _roleRepository.RemovePageAsync(roleId, pageId);
    }

    private static RoleDto ToDto(Role r)
    {
        var pages = r.RolePages
            .Select(rp => new PageDto(rp.Page.Id, rp.Page.Name, rp.Page.Description, rp.Page.Route))
            .ToList();

        return new RoleDto(r.Id, r.Name, r.Description, pages);
    }
}
