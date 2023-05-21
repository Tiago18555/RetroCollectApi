using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroCollectApi.Migrations
{
    public partial class computerandconsolecompleteprops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "UserConsoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserConsoles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnershipStatus",
                table: "UserConsoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "UserConsoles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "UserComputers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserComputers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnershipStatus",
                table: "UserComputers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "UserComputers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condition",
                table: "UserConsoles");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserConsoles");

            migrationBuilder.DropColumn(
                name: "OwnershipStatus",
                table: "UserConsoles");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "UserConsoles");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "UserComputers");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserComputers");

            migrationBuilder.DropColumn(
                name: "OwnershipStatus",
                table: "UserComputers");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "UserComputers");
        }
    }
}
