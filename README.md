# MyAuthSolution

# 🔐 MyAuthSolution - API de Autenticação (.NET 10)  Solução de autenticação e autorização utilizando **ASP.NET Core (.NET 10)**, **Entity Framework Core**, **SQL Server** e **JWT (JSON Web Tokens)**. 
--- 
## 🚀 Tecnologias e Pacotes  
**Framework:** .NET 10 (Preview/Latest)
  * **Banco de Dados:** SQL Server (LocalDB ou Instância) * **ORM:**
  *  EF Core 10 **Autenticação:** `Microsoft.AspNetCore.Authentication.JwtBearer`
  *   `Swashbuckle.AspNetCore` (v6.6.2)
      *Segurança:* Swagger configurado com `SecuritySchemeType.Http`.

 ## ️Arquitetura da Solução  O projeto está dividido em 4 camadas físicas:
  1.  **MyAuth.Domain**: Entidades (`User`), Interfaces (`IUserRepository`, `IAuthService`) e DTOs. *Zero dependências.*
  2.  **MyAuth.Data**: Implementação do EF Core (`AppDbContext`), Repositórios, Lógica JWT e **Migrations**.
  3.  **MyAuth.CrossCutting**: Configuração de Injeção de Dependência (IoC).
  4.  **MyAuth.API**: Controllers, `appsettings.json` e configuração do Swagger.

 ## 🛠️ Configuração e Instalação  
 
 ### 1. Configurar Banco de Dados
 No arquivo `MyAuth.API/appsettings.json`, ajuste a string de conexão:
   * json "ConnectionStrings": {   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyAuthDb_SQL;Trusted_Connection=True;MultipleActiveResultSets=true" }

 ### 2. Configurar JWT
 A chave usada para assinar/validar o token JWT deve ter no mínimo **256 bits** (ou seja, **32 caracteres ASCII**). Uma chave curta causa o erro `IDX10720` em runtime:

 > "Unable to create KeyedHashAlgorithm for algorithm 'http://www.w3.org/2001/04/xmldsig-more#hmac-sha256', the key size must be greater than: '256' bits, key has '136' bits."

 Edite ou substitua o segredo no `JwtSettings:Secret` por algo mais longo (por exemplo, uma string aleatória de 32+ caracteres) ou forneça via variável de ambiente em produção. A implementação já valida o comprimento e lançará uma exceção clara caso seja insuficiente.

 Exemplo de configuração válida:
 ```json
 "JwtSettings": {
   "Secret": "uX7$k9!aP0qR3sT6vWz2Yb8Nf5Hj1L0m"
 }
 ```

### 3. Modelo de permissões e roles
Para maior flexibilidade, o modelo agora armazena **roles** e **permissions** em tabelas separadas. A estrutura de tabelas é:

| Tabela | Descrição |
|--------|-----------|
| `Permissions` | Cada registro representa uma permissão individual (ex.: `ReadUsers`, `EditProfile`, `ViewSales`). Pode haver campo de descrição ou screen.
| `Roles`       | Conjuntos de permissões; também podem representar telas ou grupos lógicos.
| `UserRoles`   | associação N:N entre usuários e roles.
| `RolePermissions` | associação N:N entre roles e permissions.
| `UserPermissions` | permissões atribuídas diretamente a um usuário (opcional).

O `User` deixou de conter campos de texto para role/permissions; essas informações passam a ser recuperadas através das tabelas de junção. Os serviços carregam os relacionamentos para gerar tokens e preencher o `UserDto`, que agora expõe listas de *roles* e *permissions*.


> **Atenção:** depois de atualizar os modelos execute no projeto `MyAuth.Data` os comandos do Entity Framework:
> ```bash
> dotnet ef migrations add SplitRolesPermissions
> dotnet ef database update
> ```
>
> **Importante para macOS/Linux:** o scaffolding de migrações não funciona com o provedor LocalDB. O `AppDbContextFactory` já usa uma conexão padrão `localhost,1433` e também aceita a variável de ambiente `MYAUTH_CONNECTION` para apontar a um servidor SQL (container, Azure, etc.). Configure conforme necessário antes de executar `dotnet ef database update`.

### Novas APIs de roles e permissions

A aplicação agora inclui controladores REST dedicados para manipular *roles* e *permissions* de forma independente. Eles expõem endpoints para CRUD e também algumas rotas auxiliares que realizam os *joins* entre as tabelas -- por isso você não precisa usar as coleções de navegação diretamente.

**Exemplos de rotas:**

* `GET /api/roles` — lista todas as roles
* `GET /api/roles/{id}` — detalhes de uma role
* `POST /api/roles` — cria nova role (`RoleDto`)
* `POST /api/roles/{id}/permissions` — atribui uma permissão (envia `permissionId` no corpo)
* `GET /api/roles/{id}/permissions` — retorna nomes das permissões associadas

* `GET /api/permissions` — lista permissões
* `GET /api/permissions/{id}` — detalhes de uma permissão
* `POST /api/permissions` — cria nova permissão (`PermissionDto`)
* `GET /api/permissions/{id}/roles` — retorna nomes de roles que usam a permissão

Essas rotas usam `RoleService`, `PermissionService` e os respectivos repositórios para realizar as junções necessárias. As entidades `Role` e `Permission` permanecem enxutas e não expõem propriedades de coleção para evitar acoplamento.

> **Observação:** a tabela de usuários agora se chama `UsersSystem` no banco de dados. Essa alteração foi feita para evitar conflitos quando já exista uma tabela `Users` em uma base pré‑existente. O `DbContext` mapeia automaticamente o `User` para `UsersSystem`.

> ```

