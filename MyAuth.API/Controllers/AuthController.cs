using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAuth.Domain.Interfaces;

namespace MyAuth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Username, request.Password);

        if (token == null)
            return Unauthorized(new { message = "Usuário ou senha inválidos" });

        return Ok(new { token });
    }

    [Authorize] // Exige JWT válido
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        // Extrai o token do Header "Authorization: Bearer xyz..."
        var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        var user = await _authService.GetUserFromTokenAsync(token);
        if (user == null) return Unauthorized();

        return Ok(user);
    }
}

public record LoginRequest(string Username, string Password);