using MyAuth.Domain.Dtos;

namespace MyAuth.Domain.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(string username, string password);
    Task<UserDto?> GetUserFromTokenAsync(string token);

    // cria um novo usuário e retorna um JWT caso a operação seja bem sucedida
    // retorna null se o nome de usuário já existir
    // opcionalmente recebe uma role inicial; permissões são gerenciadas via tabelas separadas
    Task<string?> RegisterAsync(string username, string password, string initialRole = "User");
}