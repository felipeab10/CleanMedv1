using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class AddConfirmacaoAgendamentoBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ObservacaoAgendamento",
                table: "Agendamentos",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ConfirmacaoAgendamentos",
                columns: table => new
                {
                    ConfirmacaoAgendamentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoConfirmacao = table.Column<string>(nullable: false),
                    Nomecontato = table.Column<string>(nullable: true),
                    ObservacaoConfirmacao = table.Column<string>(nullable: true),
                    AgendamentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmacaoAgendamentos", x => x.ConfirmacaoAgendamentoId);
                    table.ForeignKey(
                        name: "FK_ConfirmacaoAgendamentos_Agendamentos_AgendamentoId",
                        column: x => x.AgendamentoId,
                        principalTable: "Agendamentos",
                        principalColumn: "AgendamentoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmacaoAgendamentos_AgendamentoId",
                table: "ConfirmacaoAgendamentos",
                column: "AgendamentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmacaoAgendamentos");

            migrationBuilder.AlterColumn<string>(
                name: "ObservacaoAgendamento",
                table: "Agendamentos",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
