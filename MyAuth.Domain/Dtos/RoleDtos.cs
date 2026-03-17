namespace MyAuth.Domain.Dtos;

public record RoleDto(int Id, string Name, string? Description, List<PageDto> Pages);
