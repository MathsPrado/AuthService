using MyAuth.Domain.Dtos;

namespace MyAuth.Domain.Interfaces;

public interface IPageService
{
    Task<IEnumerable<PageDto>> GetAllAsync();
    Task<PageDto?> GetByIdAsync(int id);
    Task<PageDto> CreateAsync(string name, string? description = null, string? route = null);
    Task<IEnumerable<string>> GetRolesAsync(int pageId);
    Task AssignToRoleAsync(int pageId, int roleId);
    Task RemoveFromRoleAsync(int pageId, int roleId);
    Task<IEnumerable<PageDto>> GetByRoleIdAsync(int roleId);
}
