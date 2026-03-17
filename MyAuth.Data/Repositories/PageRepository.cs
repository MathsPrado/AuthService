using Microsoft.EntityFrameworkCore;
using MyAuth.Data.Context;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Repositories;

public class PageRepository : IPageRepository
{
    private readonly AppDbContext _context;
    public PageRepository(AppDbContext context) => _context = context;

    public async Task<Page?> GetByIdAsync(int id)
        => await _context.Pages.FindAsync(id);

    public async Task<IEnumerable<Page>> GetAllAsync()
        => await _context.Pages.ToListAsync();

    public async Task AddAsync(Page page)
    {
        _context.Pages.Add(page);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetRolesAsync(int pageId)
    {
        return await _context.RolePages
            .Where(rp => rp.PageId == pageId)
            .Select(rp => rp.Role.Name)
            .ToListAsync();
    }

    public async Task AssignToRoleAsync(int pageId, int roleId)
    {
        var exists = await _context.RolePages.FindAsync(roleId, pageId);
        if (exists == null)
        {
            _context.RolePages.Add(new RolePage { RoleId = roleId, PageId = pageId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFromRoleAsync(int pageId, int roleId)
    {
        var entry = await _context.RolePages.FindAsync(roleId, pageId);
        if (entry != null)
        {
            _context.RolePages.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Page>> GetByRoleIdAsync(int roleId)
    {
        return await _context.RolePages
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.Page)
            .ToListAsync();
    }
}
