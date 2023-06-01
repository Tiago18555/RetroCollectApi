using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroCollectApi.Migrations
{
    public partial class tryingtofixnavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Computers_ComputerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Consoles_ConsoleId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ComputerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ConsoleId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ConsoleId",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComputerId",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsoleId",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ComputerId",
                table: "Games",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ConsoleId",
                table: "Games",
                column: "ConsoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Computers_ComputerId",
                table: "Games",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "ComputerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Consoles_ConsoleId",
                table: "Games",
                column: "ConsoleId",
                principalTable: "Consoles",
                principalColumn: "ConsoleId");
        }
    }
}
