using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Studentenbeheer.Migrations.Identity
{
    public partial class StudentUserAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Student",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_ApplicationUserId",
                table: "Student",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_ApplicationUserId",
                table: "Student",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_ApplicationUserId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_ApplicationUserId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Student");
        }
    }
}
