using Microsoft.EntityFrameworkCore;
using MyAuth.Data.Context;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly AppDbContext _context;
    public PermissionRepository(AppDbContext context) => _context = context;

    public async Task<Permission?> GetByIdAsync(int id)
        => await _context.Permissions.FindAsync(id);

    public async Task<IEnumerable<Permission>> GetAllAsync()
        => await _context.Permissions.ToListAsync();

    public async Task AddAsync(Permission permission)
    {
        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetRolesAsync(int permissionId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.PermissionId == permissionId)
            .Select(rp => rp.Role.Name)
            .ToListAsync();
    }
}
