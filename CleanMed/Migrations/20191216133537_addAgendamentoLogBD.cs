using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class addAgendamentoLogBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgendamentoLogs",
                columns: table => new
                {
                    AgendamentoLogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dt_Acao = table.Column<DateTime>(nullable: false),
                    Acao = table.Column<string>(nullable: false),
                    UsuarioId = table.Column<int>(nullable: false),
                    UsuarioId1 = table.Column<string>(nullable: true),
                    AgendamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendamentoLogs", x => x.AgendamentoLogId);
                    table.ForeignKey(
                        name: "FK_AgendamentoLogs_Agendamentos_AgendamentoId",
                        column: x => x.AgendamentoId,
                        principalTable: "Agendamentos",
                        principalColumn: "AgendamentoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgendamentoLogs_Usuarios_UsuarioId1",
                        column: x => x.UsuarioId1,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoLogs_AgendamentoId",
                table: "AgendamentoLogs",
                column: "AgendamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoLogs_UsuarioId1",
                table: "AgendamentoLogs",
                column: "UsuarioId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgendamentoLogs");
        }
    }
}
