namespace MyAuth.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // ex: "Admin", "Manager"
    public string? Description { get; set; }

    // se quiser associar telas ou outros metadados, adicione propriedades aqui
    public string? Screen { get; set; }

    // relationships omitted here to keep entity lean; query join tables explicitly via services
}

