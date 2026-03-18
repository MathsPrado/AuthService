namespace MyAuth.Domain.Dtos;

// Objeto limpo para devolver ao front-end (sem senha)
public record UserDto(int Id, string Username, List<RoleDto> Roles, List<PermissionDto> DirectPermissions);