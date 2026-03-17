using MyAuth.Domain.Entities;

namespace MyAuth.Domain.Interfaces;

public interface IPageRepository
{
    Task<Page?> GetByIdAsync(int id);
    Task<IEnumerable<Page>> GetAllAsync();
    Task AddAsync(Page page);
    Task<IEnumerable<string>> GetRolesAsync(int pageId);
    Task AssignToRoleAsync(int pageId, int roleId);
    Task RemoveFromRoleAsync(int pageId, int roleId);
    Task<IEnumerable<Page>> GetByRoleIdAsync(int roleId);
}
