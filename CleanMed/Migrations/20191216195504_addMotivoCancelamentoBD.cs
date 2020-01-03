using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class addMotivoCancelamentoBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotivoCancelamentos",
                columns: table => new
                {
                    MotivoCancelamentoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivoCancelamentos", x => x.MotivoCancelamentoId);
                });

            migrationBuilder.InsertData(
                table: "MotivoCancelamentos",
                columns: new[] { "MotivoCancelamentoId", "Descricao" },
                values: new object[,]
                {
                    { 1, "Cancelado pelo paciente" },
                    { 2, "Cancelado pelo Medico" },
                    { 3, "Paciente não compareceu" },
                    { 4, "OBITO DO PACIENTE" },
                    { 5, "ERRO DE DIGITACAO" },
                    { 6, "GUIA NÃO AUTORIZADA" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotivoCancelamentos");
        }
    }
}
