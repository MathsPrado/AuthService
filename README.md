# MyAuthSolution

# 🔐 MyAuthSolution - API de Autenticação (.NET 10)  Solução de autenticação e autorização utilizando **ASP.NET Core (.NET 10)**, **Entity Framework Core**, **SQL Server** e **JWT (JSON Web Tokens)**. O projeto segue uma arquitetura em camadas para garantir desacoplamento e escalabilidade.  
--- 
## 🚀 Tecnologias e Pacotes  
**Framework:** .NET 10 (Preview/Latest)
  * **Banco de Dados:** SQL Server (LocalDB ou Instância) * **ORM:**
  *  EF Core 10 (Code First) * **Autenticação:** `Microsoft.AspNetCore.Authentication.JwtBearer`
  *   `Swashbuckle.AspNetCore` (v6.6.2)
      *Segurança:* Swagger configurado com `SecuritySchemeType.Http`.

 ## ️Arquitetura da Solução  O projeto está dividido em 4 camadas físicas:
  1.  **MyAuth.Domain**: Entidades (`User`), Interfaces (`IUserRepository`, `IAuthService`) e DTOs. *Zero dependências.*
  2.  **MyAuth.Data**: Implementação do EF Core (`AppDbContext`), Repositórios, Lógica JWT e **Migrations**.
  3.  **MyAuth.CrossCutting**: Configuração de Injeção de Dependência (IoC).
  4.  **MyAuth.API**: Controllers, `appsettings.json` e configuração do Swagger.

 ## 🛠️ Configuração e Instalação  
 
 ### 1. Configurar Banco de Dados No arquivo `MyAuth.API/appsettings.json`, ajuste a string de conexão: 
   * json "ConnectionStrings": {   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyAuthDb_SQL;Trusted_Connection=True;MultipleActiveResultSets=true" }
