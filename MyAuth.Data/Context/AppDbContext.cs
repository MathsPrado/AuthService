using Microsoft.EntityFrameworkCore;
using MyAuth.Domain.Entities;

namespace MyAuth.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> UsersSystem { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<RolePage> RolePages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("UsersSystem")
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId);
        modelBuilder.Entity<UserRole>()
            .Property(ur => ur.AssignedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany() // no navigation on Role side
            .HasForeignKey(rp => rp.RoleId);
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);        modelBuilder.Entity<RolePermission>()
            .Property(rp => rp.AssignedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<UserPermission>()
            .HasKey(up => new { up.UserId, up.PermissionId });
        modelBuilder.Entity<UserPermission>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.UserId);
        modelBuilder.Entity<UserPermission>()
            .HasOne(up => up.Permission)
            .WithMany()
            .HasForeignKey(up => up.PermissionId);        modelBuilder.Entity<UserPermission>()
            .Property(up => up.AssignedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        // Page
        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Name).IsUnique();
            entity.ToTable("Screens"); // manter nome da tabela existente no banco
        });

        // RolePage (muitos-para-muitos)
        modelBuilder.Entity<RolePage>()
            .HasKey(rp => new { rp.RoleId, rp.PageId });
        modelBuilder.Entity<RolePage>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePages)
            .HasForeignKey(rp => rp.RoleId);
        modelBuilder.Entity<RolePage>()
            .HasOne(rp => rp.Page)
            .WithMany()
            .HasForeignKey(rp => rp.PageId);
        modelBuilder.Entity<RolePage>()
            .Property(rp => rp.AssignedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        modelBuilder.Entity<RolePage>()
            .ToTable("RoleScreens"); // manter nome da tabela existente no banco

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", Description = "Full access" },
            new Role { Id = 2, Name = "User", Description = "Regular user" }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = 1, Name = "ReadUsers" },
            new Permission { Id = 2, Name = "EditUsers" },
            new Permission { Id = 3, Name = "DeleteUsers" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "admin" },
            new User { Id = 2, Username = "jdoe", Password = "password" }
        );

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserId = 1, RoleId = 1, AssignedAt = new DateTime(2026,1,1) },
            new UserRole { UserId = 2, RoleId = 2, AssignedAt = new DateTime(2026,1,1) }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = 1, PermissionId = 1, AssignedAt = new DateTime(2026,1,1) },
            new RolePermission { RoleId = 1, PermissionId = 2, AssignedAt = new DateTime(2026,1,1) },
            new RolePermission { RoleId = 1, PermissionId = 3, AssignedAt = new DateTime(2026,1,1) },
            new RolePermission { RoleId = 2, PermissionId = 1, AssignedAt = new DateTime(2026,1,1) }
        );

        modelBuilder.Entity<UserPermission>().HasData(
            new UserPermission { UserId = 2, PermissionId = 2, AssignedAt = new DateTime(2026,1,1) } // individual override
        );

        // Seed Pages
        modelBuilder.Entity<Page>().HasData(
            new Page { Id = 1, Name = "Dashboard", Description = "Tela principal", Route = "/dashboard" },
            new Page { Id = 2, Name = "UserManagement", Description = "Gerenciamento de usuários", Route = "/admin/users" },
            new Page { Id = 3, Name = "RoleManagement", Description = "Gerenciamento de roles", Route = "/admin/roles" },
            new Page { Id = 4, Name = "Reports", Description = "Relatórios", Route = "/reports" }
        );

        // Admin tem acesso a todas as telas, User apenas ao Dashboard
        modelBuilder.Entity<RolePage>().HasData(
            new RolePage { RoleId = 1, PageId = 1, AssignedAt = new DateTime(2026,1,1) },
            new RolePage { RoleId = 1, PageId = 2, AssignedAt = new DateTime(2026,1,1) },
            new RolePage { RoleId = 1, PageId = 3, AssignedAt = new DateTime(2026,1,1) },
            new RolePage { RoleId = 1, PageId = 4, AssignedAt = new DateTime(2026,1,1) },
            new RolePage { RoleId = 2, PageId = 1, AssignedAt = new DateTime(2026,1,1) }
        );
    }
}