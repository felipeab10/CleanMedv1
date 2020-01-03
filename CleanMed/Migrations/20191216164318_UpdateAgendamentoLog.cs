using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class UpdateAgendamentoLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "AgendamentoLogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoLogs_PacienteId",
                table: "AgendamentoLogs",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendamentoLogs_Pacientes_PacienteId",
                table: "AgendamentoLogs",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendamentoLogs_Pacientes_PacienteId",
                table: "AgendamentoLogs");

            migrationBuilder.DropIndex(
                name: "IX_AgendamentoLogs_PacienteId",
                table: "AgendamentoLogs");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "AgendamentoLogs");
        }
    }
}
