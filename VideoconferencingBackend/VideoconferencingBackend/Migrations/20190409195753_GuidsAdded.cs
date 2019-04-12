using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoconferencingBackend.Migrations
{
    public partial class GuidsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserGuid",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupGuid",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGuid",
                table: "Users",
                column: "UserGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupGuid",
                table: "Groups",
                column: "GroupGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Name",
                table: "Groups",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Login",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserGuid",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupGuid",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Name",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupGuid",
                table: "Groups");
        }
    }
}
