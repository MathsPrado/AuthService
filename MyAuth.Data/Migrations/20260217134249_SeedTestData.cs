using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyAuth.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // additional permissions (existing FullAccess at Id=1 from initial migration)
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 2, null, "ReadUsers" },
                    { 3, null, "EditUsers" },
                    { 4, null, "DeleteUsers" }
                });

            // role Admin already seeded (Id=1) in initial migration
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Description", "Name", "Screen" },
                values: new object[,]
                {
                    { 2, "Regular user", "User", null }
                });

            migrationBuilder.InsertData(
                table: "UsersSystem",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "admin123", "admin" },
                    { 2, "password", "jdoe" },
                    { 3, "secret", "alice" }
                });

            // RolePermissions for new permissions/roles
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "AssignedAt" },
                values: new object[,]
                {
                    // give User role read access
                    { 2, 2, new DateTime(2026,1,1) }
                });

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "PermissionId", "UserId", "AssignedAt" },
                values: new object[] { 3, 3, new DateTime(2026,1,1) });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "AssignedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1) },
                    { 2, 2, new DateTime(2026, 1, 1) },
                    { 2, 3, new DateTime(2026, 1, 1) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remove the RolePermission we added (User role read access)
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UserPermissions",
                keyColumns: new[] { "PermissionId", "UserId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleI1, 1 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { d", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UsersSystem",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UsersSystem",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UsersSystem",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
