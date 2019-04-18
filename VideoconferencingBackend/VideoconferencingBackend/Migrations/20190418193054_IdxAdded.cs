using Microsoft.EntityFrameworkCore.Migrations;

namespace VideoconferencingBackend.Migrations
{
    public partial class IdxAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupInCallId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupInCallId",
                table: "Users",
                column: "GroupInCallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupInCallId",
                table: "Users",
                column: "GroupInCallId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupInCallId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupInCallId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupInCallId",
                table: "Users");
        }
    }
}
