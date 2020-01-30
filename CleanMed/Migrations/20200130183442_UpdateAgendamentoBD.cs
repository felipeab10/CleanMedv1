using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class UpdateAgendamentoBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_AtendimentoId",
                table: "Agendamentos",
                column: "AtendimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Atendimentos_AtendimentoId",
                table: "Agendamentos",
                column: "AtendimentoId",
                principalTable: "Atendimentos",
                principalColumn: "AtendimentoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Atendimentos_AtendimentoId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_AtendimentoId",
                table: "Agendamentos");
        }
    }
}
