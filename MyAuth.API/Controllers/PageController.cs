using Microsoft.AspNetCore.Mvc;
using MyAuth.Domain.Dtos;
using MyAuth.Domain.Interfaces;

namespace MyAuth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PageController : ControllerBase
{
    private readonly IPageService _pageService;

    public PageController(IPageService pageService) => _pageService = pageService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pages = await _pageService.GetAllAsync();
        return Ok(pages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _pageService.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PageDto dto)
    {
        var created = await _pageService.CreateAsync(dto.Name, dto.Description, dto.Route);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRoles(int id)
    {
        var list = await _pageService.GetRolesAsync(id);
        return Ok(list);
    }

    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AssignToRole(int id, [FromBody] int roleId)
    {
        await _pageService.AssignToRoleAsync(id, roleId);
        return NoContent();
    }

    [HttpDelete("{id}/roles/{roleId}")]
    public async Task<IActionResult> RemoveFromRole(int id, int roleId)
    {
        await _pageService.RemoveFromRoleAsync(id, roleId);
        return NoContent();
    }
}
