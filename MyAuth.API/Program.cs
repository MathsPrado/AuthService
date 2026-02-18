using Microsoft.OpenApi.Models;
using MyAuth.CrossCutting.IoC;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura��o Global (Banco, Auth, Depend�ncias)
builder.Services.RegisterServices(builder.Configuration, builder.Environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Configura��o do Swagger com Suporte a JWT (Cadeado)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAuth.API", Version = "v1" });

    // --- ALTERA��O: Usando Type = Http em vez de ApiKey ---
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Cole seu token JWT aqui (n�o precisa escrever 'Bearer')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, // <--- Isso faz a m�gica
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// 3. Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Quem � voc�?
app.UseAuthorization();  // O que voc� pode fazer?

app.MapControllers();

app.Run();

// Make the Program class accessible to integration tests
public partial class Program { }