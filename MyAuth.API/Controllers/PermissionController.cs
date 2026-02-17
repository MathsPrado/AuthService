using Microsoft.AspNetCore.Mvc;
using MyAuth.Domain.Dtos;
using MyAuth.Domain.Interfaces;

namespace MyAuth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService) => _permissionService = permissionService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var perms = await _permissionService.GetAllAsync();
        return Ok(perms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _permissionService.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PermissionDto dto)
    {
        var created = await _permissionService.CreateAsync(dto.Name, dto.Description);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRoles(int id)
    {
        var list = await _permissionService.GetRolesAsync(id);
        return Ok(list);
    }
}
