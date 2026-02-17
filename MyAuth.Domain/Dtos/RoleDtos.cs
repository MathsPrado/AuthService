namespace MyAuth.Domain.Dtos;

// Representação leve de uma role ou permissão
public record RoleDto(int Id, string Name, string? Description);
