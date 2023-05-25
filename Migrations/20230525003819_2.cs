using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroCollectApi.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Computers_ComputerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Consoles_ConsoleId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "ComputerId",
                table: "UserCollections",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConsoleId",
                table: "UserCollections",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ConsoleId",
                table: "Games",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "ComputerId",
                table: "Games",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<List<string>>(
                name: "Genres",
                table: "Games",
                type: "text[]",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Computers_ComputerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Consoles_ConsoleId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "UserCollections");

            migrationBuilder.DropColumn(
                name: "ConsoleId",
                table: "UserCollections");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "ConsoleId",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ComputerId",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Games",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Computers_ComputerId",
                table: "Games",
                column: "ComputerId",
                principalTable: "Computers",
                principalColumn: "ComputerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Consoles_ConsoleId",
                table: "Games",
                column: "ConsoleId",
                principalTable: "Consoles",
                principalColumn: "ConsoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
