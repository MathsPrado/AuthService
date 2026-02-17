# MyAuthSolution

## Authentication and Authorization with .NET 10

MyAuthSolution provides a clean, modular starting point for building authentication
and authorization services in ASP.NET Core. It combines JWT tokens, EF Core
persistence and a flexible roles/permissions model, and ships with Swagger
documentation out of the box.

Key features:

- Symmetric JWT tokens secured with 256-bit secret
- Layered architecture (Domain / Data / CrossCutting / API)
- Roles and permissions stored in normalized tables with many-to-many joins
- RESTful controllers for users, roles and permissions
- EF Core migrations compatible with Docker/remote SQL Server

---

## Technologies

Component             | Details
--------------------- | ----------------------------------------
.NET                  | 10 (latest stable release)
Entity Framework Core | Version 10
Database              | Microsoft SQL Server (LocalDB, container or remote)
Authentication        | Microsoft.AspNetCore.Authentication.JwtBearer
Documentation         | Swagger / Swashbuckle

---

## Solution Structure

The repository contains four projects:

* **MyAuth.Domain** – domain entities, DTOs and interfaces. No external
  dependencies.
* **MyAuth.Data** – EF Core DbContext, repositories, services and migrations.
* **MyAuth.CrossCutting** – dependency injection configuration.
* **MyAuth.API** – ASP.NET Core application, controllers and middleware.

This separation keeps business rules independent of infrastructure code and
facilitates testing.

---

## Setup

1. **Configure the database**

   Edit `MyAuth.API/appsettings.json` or set the environment variable
   `MYAUTH_CONNECTION` with a valid SQL Server connection string. Example:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost,1433;Database=MyAuthDb_SQL;User Id=sa;Password=Your_password123;"
   }
   ```

   On macOS/Linux the LocalDB provider is not supported, so target a remote
   or containerized instance instead.

2. **Set the JWT secret**

   The JWT secret must be at least 256 bits (32 ASCII characters); a shorter
   value will trigger an `IDX10720` error at runtime. The secret goes under
   `JwtSettings:Secret` in configuration. The `NativeInjector` class validates
   the length automatically.

   ```json
   "JwtSettings": {
     "Secret": "your-32-or-more-character-secret"
   }
   ```

3. **Apply migrations**

   From the `MyAuth.Data` project directory run:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

   Whenever the data model changes (for example, when adding roles and
   permissions) create and apply a new migration. If you are using a container
   or Azure SQL, export `MYAUTH_CONNECTION` first so the design-time factory
   uses the correct server.

---

## Roles and Permissions Model

Permissions and roles are stored in separate tables with many-to-many
junction tables:

* `Permissions` – individual permissions such as `ReadUsers` or `EditProfile`.
* `Roles` – groups of permissions.
* `UserRoles`, `RolePermissions` and `UserPermissions` – join tables.

Users no longer carry text fields for roles or permissions; the services load
related entities and populate the `UserDto` with lists of role names and
permission names.

Note: the users table is named `UsersSystem` in the database to avoid conflicts
with existing `Users` tables. Mapping is handled automatically by `AppDbContext`.

---

## API Endpoints

Path                             | Method        | Description
--------------------------------- | ------------- | -------------------------------
`/api/auth/login`                 | POST          | Authenticate and obtain JWT token
`/api/auth/register`              | POST          | Register a new user
`/api/roles`                      | GET, POST     | List or create roles
`/api/roles/{id}`                 | GET, PUT, DELETE | CRUD operations on a role
`/api/roles/{id}/permissions`     | POST, GET, DELETE | Manage role permissions
`/api/permissions`                | GET, POST     | List or create permissions
`/api/permissions/{id}`           | GET, PUT, DELETE | CRUD operations on a permission
`/api/permissions/{id}/roles`     | GET           | Roles associated with a permission

Swagger UI is available at `/swagger` when the API is running.

---

## Running the Application

From the solution root:

```bash
cd MyAuth.API

dotnet run
```

The API will listen on `https://localhost:5001` by default (see
`launchSettings.json`).

---

## Contributing

Pull requests and suggestions are welcome. Possible improvements include:

* Unit and integration tests.
* Refresh token support.
* External identity providers (OAuth, OpenID Connect).

---

## License

This repository is a sample application. Feel free to adapt it for your own
projects.

---

Thank you for using MyAuthSolution – built with care to demonstrate modern .NET
practices.

