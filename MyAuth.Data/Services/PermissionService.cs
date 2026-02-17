using MyAuth.Domain.Dtos;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;

    public PermissionService(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<IEnumerable<PermissionDto>> GetAllAsync()
    {
        var list = await _permissionRepository.GetAllAsync();
        return list.Select(p => new PermissionDto(p.Id, p.Name, p.Description));
    }

    public async Task<PermissionDto?> GetByIdAsync(int id)
    {
        var p = await _permissionRepository.GetByIdAsync(id);
        if (p == null) return null;
        return new PermissionDto(p.Id, p.Name, p.Description);
    }

    public async Task<PermissionDto> CreateAsync(string name, string? description = null)
    {
        var permission = new Permission { Name = name, Description = description };
        await _permissionRepository.AddAsync(permission);
        return new PermissionDto(permission.Id, permission.Name, permission.Description);
    }

    public async Task<IEnumerable<string>> GetRolesAsync(int permissionId)
    {
        return await _permissionRepository.GetRolesAsync(permissionId);
    }
}
