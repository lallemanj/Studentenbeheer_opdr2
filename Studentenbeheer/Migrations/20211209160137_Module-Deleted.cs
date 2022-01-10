using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studentenbeheer.Migrations
{
    public partial class ModuleDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inschrijvingen_Module_ModuleId",
                table: "Inschrijvingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Inschrijvingen_Student_StudentID",
                table: "Inschrijvingen");

            migrationBuilder.DropIndex(
                name: "IX_Inschrijvingen_ModuleId",
                table: "Inschrijvingen");

            migrationBuilder.DropIndex(
                name: "IX_Inschrijvingen_StudentID",
                table: "Inschrijvingen");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Inschrijvingen");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "Inschrijvingen");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Student",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "InschrijvingenId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Module",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "InschrijvingenModule",
                columns: table => new
                {
                    InschrijvingenLijstId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InschrijvingenModule", x => new { x.InschrijvingenLijstId, x.ModuleId });
                    table.ForeignKey(
                        name: "FK_InschrijvingenModule_Inschrijvingen_InschrijvingenLijstId",
                        column: x => x.InschrijvingenLijstId,
                        principalTable: "Inschrijvingen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InschrijvingenModule_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_InschrijvingenId",
                table: "Student",
                column: "InschrijvingenId");

            migrationBuilder.CreateIndex(
                name: "IX_InschrijvingenModule_ModuleId",
                table: "InschrijvingenModule",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Inschrijvingen_InschrijvingenId",
                table: "Student",
                column: "InschrijvingenId",
                principalTable: "Inschrijvingen",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Inschrijvingen_InschrijvingenId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "InschrijvingenModule");

            migrationBuilder.DropIndex(
                name: "IX_Student_InschrijvingenId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "InschrijvingenId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Module");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Inschrijvingen",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentID",
                table: "Inschrijvingen",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_ModuleId",
                table: "Inschrijvingen",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_StudentID",
                table: "Inschrijvingen",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Inschrijvingen_Module_ModuleId",
                table: "Inschrijvingen",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inschrijvingen_Student_StudentID",
                table: "Inschrijvingen",
                column: "StudentID",
                principalTable: "Student",
                principalColumn: "ID");
        }
    }
}
