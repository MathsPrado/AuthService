using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyAuth.Data.Context;
using MyAuth.Data.Repositories;
using MyAuth.Data.Services;
using MyAuth.Domain.Interfaces;

namespace MyAuth.CrossCutting.IoC;

public static class NativeInjector
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {

        // 1. Banco de Dados (Com a correção do MigrationsAssembly)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("MyAuth.Data") // <--- O SEGREDO ESTÁ AQUI
            ));

        // 2. Injeção de Dependência (Repositories e Services)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        // role/permission support
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IPageRepository, PageRepository>();
        services.AddScoped<IPageService, PageService>();

        // 3. Configuração JWT (Segurança)
        var secretKey = configuration["JwtSettings:Secret"];

        // O algoritmo HMACSHA256 exige uma chave de, no mínimo, 256 bits (32 bytes).
        // Se a configuração estiver zerada ou muito curta teremos uma exceção IDX10720.
        if (string.IsNullOrEmpty(secretKey) || Encoding.ASCII.GetByteCount(secretKey) < 32)
        {
            throw new InvalidOperationException("JwtSettings:Secret must be at least 32 ASCII characters (256 bits) to use HmacSha256. " +
                                                "Configure a stronger key in appsettings or via environment variables.");
        }

        var key = Encoding.ASCII.GetBytes(secretKey);

        services.AddAuthentication(x => {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x => {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }
}