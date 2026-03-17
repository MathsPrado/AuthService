using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAuth.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameScreenToPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScreens_Screens_ScreenId",
                table: "RoleScreens");

            migrationBuilder.RenameColumn(
                name: "ScreenId",
                table: "RoleScreens",
                newName: "PageId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleScreens_ScreenId",
                table: "RoleScreens",
                newName: "IX_RoleScreens_PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScreens_Screens_PageId",
                table: "RoleScreens",
                column: "PageId",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleScreens_Screens_PageId",
                table: "RoleScreens");

            migrationBuilder.RenameColumn(
                name: "PageId",
                table: "RoleScreens",
                newName: "ScreenId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleScreens_PageId",
                table: "RoleScreens",
                newName: "IX_RoleScreens_ScreenId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleScreens_Screens_ScreenId",
                table: "RoleScreens",
                column: "ScreenId",
                principalTable: "Screens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
