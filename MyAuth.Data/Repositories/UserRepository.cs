using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyAuth.Data.Context;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.UsersSystem
            .FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.UsersSystem
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePages).ThenInclude(rp => rp.Page)
            .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _context.UsersSystem
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.RolePages).ThenInclude(rp => rp.Page)
            .Include(u => u.UserPermissions).ThenInclude(up => up.Permission)
            .ToListAsync();

    public async Task AddAsync(User user)
    {
        _context.UsersSystem.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task AssignRoleAsync(int userId, int roleId)
    {
        var exists = await _context.UserRoles.FindAsync(userId, roleId);
        if (exists == null)
        {
            _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveRoleAsync(int userId, int roleId)
    {
        var entity = await _context.UserRoles.FindAsync(userId, roleId);
        if (entity != null)
        {
            _context.UserRoles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AssignPermissionAsync(int userId, int permissionId)
    {
        var exists = await _context.UserPermissions.FindAsync(userId, permissionId);
        if (exists == null)
        {
            _context.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = permissionId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemovePermissionAsync(int userId, int permissionId)
    {
        var entity = await _context.UserPermissions.FindAsync(userId, permissionId);
        if (entity != null)
        {
            _context.UserPermissions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<string>> GetRolesAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetPermissionsAsync(int userId)
    {
        // combine direct permissions and those via roles
        var direct = _context.UserPermissions
            .Where(up => up.UserId == userId)
            .Select(up => up.Permission.Name);

        var viaRoles = from ur in _context.UserRoles
                       where ur.UserId == userId
                       join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                       select rp.Permission.Name;

        return await direct.Union(viaRoles).ToListAsync();
    }
}