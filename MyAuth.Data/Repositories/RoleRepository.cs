using Microsoft.EntityFrameworkCore;
using MyAuth.Data.Context;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    public RoleRepository(AppDbContext context) => _context = context;

    public async Task<Role?> GetByIdAsync(int id)
        => await _context.Roles
            .Include(r => r.RolePages)
                .ThenInclude(rp => rp.Page)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Role>> GetAllAsync()
        => await _context.Roles
            .Include(r => r.RolePages)
                .ThenInclude(rp => rp.Page)
            .ToListAsync();

    public async Task AddAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
    }

    public async Task AssignPermissionAsync(int roleId, int permissionId)
    {
        var exists = await _context.RolePermissions.FindAsync(roleId, permissionId);
        if (exists == null)
        {
            _context.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<string>> GetPermissionsAsync(int roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.Permission.Name)
            .ToListAsync();
    }

    public async Task AssignPageAsync(int roleId, int pageId)
    {
        var exists = await _context.RolePages.FindAsync(roleId, pageId);
        if (exists == null)
        {
            _context.RolePages.Add(new RolePage { RoleId = roleId, PageId = pageId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemovePageAsync(int roleId, int pageId)
    {
        var entry = await _context.RolePages.FindAsync(roleId, pageId);
        if (entry != null)
        {
            _context.RolePages.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
