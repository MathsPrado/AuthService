using MyAuth.Domain.Dtos;

namespace MyAuth.Domain.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(string username, string password);
    Task<UserDto?> GetUserFromTokenAsync(string token);
}