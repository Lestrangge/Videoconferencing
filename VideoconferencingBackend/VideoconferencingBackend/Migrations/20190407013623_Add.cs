using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoconferencingBackend.Migrations
{
    public partial class Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsAndUsers_Groups_GroupId",
                table: "GroupsAndUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupsAndUsers_Users_UserId",
                table: "GroupsAndUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupsAndUsers",
                table: "GroupsAndUsers");

            migrationBuilder.RenameTable(
                name: "GroupsAndUsers",
                newName: "GroupUsers");

            migrationBuilder.RenameIndex(
                name: "IX_GroupsAndUsers_UserId",
                table: "GroupUsers",
                newName: "IX_GroupUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupUsers",
                table: "GroupUsers",
                columns: new[] { "GroupId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Users_UserId",
                table: "GroupUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Users_UserId",
                table: "GroupUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupUsers",
                table: "GroupUsers");

            migrationBuilder.RenameTable(
                name: "GroupUsers",
                newName: "GroupsAndUsers");

            migrationBuilder.RenameIndex(
                name: "IX_GroupUsers_UserId",
                table: "GroupsAndUsers",
                newName: "IX_GroupsAndUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupsAndUsers",
                table: "GroupsAndUsers",
                columns: new[] { "GroupId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsAndUsers_Groups_GroupId",
                table: "GroupsAndUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsAndUsers_Users_UserId",
                table: "GroupsAndUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
