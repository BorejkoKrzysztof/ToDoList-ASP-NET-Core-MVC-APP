using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListInfrastructure.Database.Migrations
{
    public partial class todoentry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ToDoEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ToDoEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "ToDoEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "ToDoEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ToDoEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ToDoEntries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ToDoEntries");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ToDoEntries");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "ToDoEntries");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "ToDoEntries");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ToDoEntries");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ToDoEntries");
        }
    }
}
