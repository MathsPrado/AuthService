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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Mapear a entidade User para a tabela UsersSystem e garantir índice único no Username
        modelBuilder.Entity<User>()
            .ToTable("UsersSystem")
            .HasIndex(u => u.Username)
            .IsUnique();

        // configurações many-to-many using join entities
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany() // no navigation on Role side
            .HasForeignKey(ur => ur.RoleId);        // default SQL timestamp for audit field
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
            .WithMany() // no navigation on Permission side
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
            .WithMany() // no navigation on Permission side
            .HasForeignKey(up => up.PermissionId);        modelBuilder.Entity<UserPermission>()
            .Property(up => up.AssignedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        // seed data for testing/demo
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
    }
}