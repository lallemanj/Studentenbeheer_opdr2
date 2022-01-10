using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studentenbeheer.Migrations.Identity
{
    public partial class lastupdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inschrijvingen_Module_ModuleId",
                table: "Inschrijvingen");

            migrationBuilder.DropIndex(
                name: "IX_Inschrijvingen_ModuleId",
                table: "Inschrijvingen");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Inschrijvingen");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deleted",
                table: "Student",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deleted",
                table: "Module",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_ModuleIds",
                table: "Inschrijvingen",
                column: "ModuleIds");

            migrationBuilder.AddForeignKey(
                name: "FK_Inschrijvingen_Module_ModuleIds",
                table: "Inschrijvingen",
                column: "ModuleIds",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inschrijvingen_Module_ModuleIds",
                table: "Inschrijvingen");

            migrationBuilder.DropIndex(
                name: "IX_Inschrijvingen_ModuleIds",
                table: "Inschrijvingen");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deleted",
                table: "Student",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deleted",
                table: "Module",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Inschrijvingen",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inschrijvingen_ModuleId",
                table: "Inschrijvingen",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inschrijvingen_Module_ModuleId",
                table: "Inschrijvingen",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id");
        }
    }
}
