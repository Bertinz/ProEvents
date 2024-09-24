using Microsoft.EntityFrameworkCore.Migrations;

namespace ProEvents.Persistence.Migrations
{
    public partial class testeAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lotes_Eventos_EventoId",
                table: "Lotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PalestrantesEventos_Eventos_EventoId",
                table: "PalestrantesEventos");

            migrationBuilder.DropForeignKey(
                name: "FK_RedesSociais_Eventos_EventoId",
                table: "RedesSociais");

            migrationBuilder.DropForeignKey(
                name: "FK_RedesSociais_Palestrantes_PalestranteId",
                table: "RedesSociais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Eventos",
                table: "Eventos");

            migrationBuilder.RenameTable(
                name: "Eventos",
                newName: "EventosDetalhes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventosDetalhes",
                table: "EventosDetalhes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lotes_EventosDetalhes_EventoId",
                table: "Lotes",
                column: "EventoId",
                principalTable: "EventosDetalhes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PalestrantesEventos_EventosDetalhes_EventoId",
                table: "PalestrantesEventos",
                column: "EventoId",
                principalTable: "EventosDetalhes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RedesSociais_EventosDetalhes_EventoId",
                table: "RedesSociais",
                column: "EventoId",
                principalTable: "EventosDetalhes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RedesSociais_Palestrantes_PalestranteId",
                table: "RedesSociais",
                column: "PalestranteId",
                principalTable: "Palestrantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lotes_EventosDetalhes_EventoId",
                table: "Lotes");

            migrationBuilder.DropForeignKey(
                name: "FK_PalestrantesEventos_EventosDetalhes_EventoId",
                table: "PalestrantesEventos");

            migrationBuilder.DropForeignKey(
                name: "FK_RedesSociais_EventosDetalhes_EventoId",
                table: "RedesSociais");

            migrationBuilder.DropForeignKey(
                name: "FK_RedesSociais_Palestrantes_PalestranteId",
                table: "RedesSociais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventosDetalhes",
                table: "EventosDetalhes");

            migrationBuilder.RenameTable(
                name: "EventosDetalhes",
                newName: "Eventos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Eventos",
                table: "Eventos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lotes_Eventos_EventoId",
                table: "Lotes",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PalestrantesEventos_Eventos_EventoId",
                table: "PalestrantesEventos",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RedesSociais_Eventos_EventoId",
                table: "RedesSociais",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RedesSociais_Palestrantes_PalestranteId",
                table: "RedesSociais",
                column: "PalestranteId",
                principalTable: "Palestrantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
