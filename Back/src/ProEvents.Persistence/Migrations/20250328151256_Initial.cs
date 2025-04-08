using Microsoft.EntityFrameworkCore.Migrations;

namespace ProEvents.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Palestrantes_AspNetUsers_UserId1",
                table: "Palestrantes");

            migrationBuilder.DropIndex(
                name: "IX_Palestrantes_UserId1",
                table: "Palestrantes");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Palestrantes");

            migrationBuilder.DropColumn(
                name: "PalestranteId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Palestrantes_UserId",
                table: "Palestrantes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Palestrantes_AspNetUsers_UserId",
                table: "Palestrantes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Palestrantes_AspNetUsers_UserId",
                table: "Palestrantes");

            migrationBuilder.DropIndex(
                name: "IX_Palestrantes_UserId",
                table: "Palestrantes");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Palestrantes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PalestranteId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Palestrantes_UserId1",
                table: "Palestrantes",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Palestrantes_AspNetUsers_UserId1",
                table: "Palestrantes",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
