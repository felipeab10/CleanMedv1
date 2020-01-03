using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class addcampoMotivoCancelamentoIdAgendamentoBd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MotivoCancelamentoId",
                table: "Agendamentos",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MotivoCancelamentos",
                keyColumn: "MotivoCancelamentoId",
                keyValue: 1,
                column: "Descricao",
                value: "CANCELADO PELO PACIENTE");

            migrationBuilder.UpdateData(
                table: "MotivoCancelamentos",
                keyColumn: "MotivoCancelamentoId",
                keyValue: 2,
                column: "Descricao",
                value: "CANCELADO PELO MÉDICO(A)");

            migrationBuilder.UpdateData(
                table: "MotivoCancelamentos",
                keyColumn: "MotivoCancelamentoId",
                keyValue: 3,
                column: "Descricao",
                value: "PACIENTE NÃO COMPARECEU");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_MotivoCancelamentoId",
                table: "Agendamentos",
                column: "MotivoCancelamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_MotivoCancelamentos_MotivoCancelamentoId",
                table: "Agendamentos",
                column: "MotivoCancelamentoId",
                principalTable: "MotivoCancelamentos",
                principalColumn: "MotivoCancelamentoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_MotivoCancelamentos_MotivoCancelamentoId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_MotivoCancelamentoId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "MotivoCancelamentoId",
                table: "Agendamentos");

            migrationBuilder.UpdateData(
                table: "MotivoCancelamentos",
                keyColumn: "MotivoCancelamentoId",
                keyValue: 1,
                column: "Descricao",
                value: "Cancelado pelo paciente");

            migrationBuilder.UpdateData(
                table: "MotivoCancelamentos",
                keyColumn: "MotivoCancelamentoId",
                keyValue: 2,
                column: "Descricao",
                value: "Cancelado pelo Medico");

            migrationBuilder.UpdateData(
                table: "MotivoCancelamentos",
                keyColumn: "MotivoCancelamentoId",
                keyValue: 3,
                column: "Descricao",
                value: "Paciente não compareceu");
        }
    }
}
