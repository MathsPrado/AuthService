using Microsoft.AspNetCore.Mvc;
using MyAuth.Domain.Dtos;
using MyAuth.Domain.Interfaces;

namespace MyAuth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleDto dto)
    {
        var created = await _roleService.CreateAsync(dto.Name, dto.Description);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> AssignPermission(int id, [FromBody] int permissionId)
    {
        await _roleService.AssignPermissionAsync(id, permissionId);
        return NoContent();
    }

    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetPermissions(int id)
    {
        var list = await _roleService.GetPermissionsAsync(id);
        return Ok(list);
    }

    [HttpPost("{id}/pages")]
    public async Task<IActionResult> AssignPage(int id, [FromBody] int pageId)
    {
        await _roleService.AssignPageAsync(id, pageId);
        return NoContent();
    }

    [HttpDelete("{id}/pages/{pageId}")]
    public async Task<IActionResult> RemovePage(int id, int pageId)
    {
        await _roleService.RemovePageAsync(id, pageId);
        return NoContent();
    }
}
