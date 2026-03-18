using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyAuth.Domain.Dtos;
using MyAuth.Domain.Entities;
using MyAuth.Domain.Interfaces;

namespace MyAuth.Data.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        // Validação de senha simples. 
        if (user == null || user.Password != password) return null;

        var roles = await _userRepository.GetRolesAsync(user.Id);
        var permissions = await _userRepository.GetPermissionsAsync(user.Id);
        return GenerateJwtToken(user, roles, permissions);
    }

    public async Task<UserDto?> GetUserFromTokenAsync(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (idClaim == null) return null;

            var user = await _userRepository.GetByIdAsync(int.Parse(idClaim));
            if (user == null) return null;

            var roles = await _userRepository.GetRolesAsync(user.Id);
            var permissions = await _userRepository.GetPermissionsAsync(user.Id);

            var roleDtos = roles.Select(r => new RoleDto(0, r, null, [])).ToList();
            var permDtos = permissions.Select(p => new PermissionDto(0, p, null)).ToList();

            return new UserDto(user.Id, user.Username, roleDtos, permDtos);
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> RegisterAsync(string username, string password, string initialRole = "User")
    {
        // verifica se já existe
        if (await _userRepository.GetByUsernameAsync(username) != null)
            return null;

        var user = new User
        {
            Username = username,
            Password = password
        };

        await _userRepository.AddAsync(user);

        // opcionalmente atribuir role via outro serviço

        // retorna token de acesso igual ao login automático
        var roles = await _userRepository.GetRolesAsync(user.Id);
        var permissions = await _userRepository.GetPermissionsAsync(user.Id);
        return GenerateJwtToken(user, roles, permissions);
    }

    private string GenerateJwtToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        // A chave vem do appsettings da API
        var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Secret"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("id", user.Id.ToString())
        };
        // adicionar múltiplos roles e permissões
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}