using Microsoft.AspNetCore.Mvc;
using MyAuth.Domain.Interfaces;

namespace MyAuth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // POST api/user/1/roles  body: 2
    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AssignRole(int id, [FromBody] int roleId)
    {
        await _userService.AssignRoleAsync(id, roleId);
        return NoContent();
    }

    // DELETE api/user/1/roles/2
    [HttpDelete("{id}/roles/{roleId}")]
    public async Task<IActionResult> RemoveRole(int id, int roleId)
    {
        await _userService.RemoveRoleAsync(id, roleId);
        return NoContent();
    }

    // POST api/user/1/permissions  body: 3
    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> AssignPermission(int id, [FromBody] int permissionId)
    {
        await _userService.AssignPermissionAsync(id, permissionId);
        return NoContent();
    }

    // DELETE api/user/1/permissions/3
    [HttpDelete("{id}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermission(int id, int permissionId)
    {
        await _userService.RemovePermissionAsync(id, permissionId);
        return NoContent();
    }
}
