using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class AtendimentoBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atendimentos",
                columns: table => new
                {
                    AtendimentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(nullable: false),
                    SetorId = table.Column<int>(nullable: false),
                    ConvenioId = table.Column<int>(nullable: false),
                    DataAtendimento = table.Column<DateTime>(nullable: false),
                    HoraAtendimento = table.Column<DateTime>(nullable: false),
                    PrestadorId = table.Column<int>(nullable: true),
                    NrCarteira = table.Column<string>(nullable: false),
                    SnEmAtendimento = table.Column<bool>(nullable: true),
                    MyProperty = table.Column<int>(nullable: false),
                    PrioridadeAtendimento = table.Column<string>(nullable: true),
                    SnRetorno = table.Column<bool>(nullable: true),
                    SnAcompanhante = table.Column<bool>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atendimentos", x => x.AtendimentoId);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Convenios_ConvenioId",
                        column: x => x.ConvenioId,
                        principalTable: "Convenios",
                        principalColumn: "ConvenioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "PacienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Prestadores_PrestadorId",
                        column: x => x.PrestadorId,
                        principalTable: "Prestadores",
                        principalColumn: "PrestadorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "SetorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_ConvenioId",
                table: "Atendimentos",
                column: "ConvenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_PacienteId",
                table: "Atendimentos",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_PrestadorId",
                table: "Atendimentos",
                column: "PrestadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_SetorId",
                table: "Atendimentos",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_UsuarioId",
                table: "Atendimentos",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atendimentos");
        }
    }
}
