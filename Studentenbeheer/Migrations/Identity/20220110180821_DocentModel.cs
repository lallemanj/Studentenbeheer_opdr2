using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studentenbeheer.Migrations.Identity
{
    public partial class DocentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Docent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeslachtId = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Docent_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Docent_Gender_GeslachtId",
                        column: x => x.GeslachtId,
                        principalTable: "Gender",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocentModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    DocentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocentModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocentModule_Docent_DocentId",
                        column: x => x.DocentId,
                        principalTable: "Docent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocentModule_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Docent_ApplicationUserId",
                table: "Docent",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Docent_GeslachtId",
                table: "Docent",
                column: "GeslachtId");

            migrationBuilder.CreateIndex(
                name: "IX_DocentModule_DocentId",
                table: "DocentModule",
                column: "DocentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocentModule_ModuleId",
                table: "DocentModule",
                column: "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocentModule");

            migrationBuilder.DropTable(
                name: "Docent");
        }
    }
}
