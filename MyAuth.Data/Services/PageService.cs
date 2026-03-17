using MyAuth.Domain.Dtos;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Services;

public class PageService : IPageService
{
    private readonly IPageRepository _pageRepository;

    public PageService(IPageRepository pageRepository)
    {
        _pageRepository = pageRepository;
    }

    public async Task<IEnumerable<PageDto>> GetAllAsync()
    {
        var list = await _pageRepository.GetAllAsync();
        return list.Select(p => new PageDto(p.Id, p.Name, p.Description, p.Route));
    }

    public async Task<PageDto?> GetByIdAsync(int id)
    {
        var p = await _pageRepository.GetByIdAsync(id);
        if (p == null) return null;
        return new PageDto(p.Id, p.Name, p.Description, p.Route);
    }

    public async Task<PageDto> CreateAsync(string name, string? description = null, string? route = null)
    {
        var page = new Page { Name = name, Description = description, Route = route };
        await _pageRepository.AddAsync(page);
        return new PageDto(page.Id, page.Name, page.Description, page.Route);
    }

    public async Task<IEnumerable<string>> GetRolesAsync(int pageId)
    {
        return await _pageRepository.GetRolesAsync(pageId);
    }

    public async Task AssignToRoleAsync(int pageId, int roleId)
    {
        await _pageRepository.AssignToRoleAsync(pageId, roleId);
    }

    public async Task RemoveFromRoleAsync(int pageId, int roleId)
    {
        await _pageRepository.RemoveFromRoleAsync(pageId, roleId);
    }

    public async Task<IEnumerable<PageDto>> GetByRoleIdAsync(int roleId)
    {
        var list = await _pageRepository.GetByRoleIdAsync(roleId);
        return list.Select(p => new PageDto(p.Id, p.Name, p.Description, p.Route));
    }
}
