using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListInfrastructure.Database.Migrations
{
    public partial class addNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notes_ToDoEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToDoEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes_ToDoEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_ToDoEntry_ToDoEntries_ToDoEntryId",
                        column: x => x.ToDoEntryId,
                        principalTable: "ToDoEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ToDoEntry_ToDoEntryId",
                table: "Notes_ToDoEntry",
                column: "ToDoEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes_ToDoEntry");
        }
    }
}
