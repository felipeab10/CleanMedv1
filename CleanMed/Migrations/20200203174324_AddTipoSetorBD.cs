using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class AddTipoSetorBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoSetores",
                columns: table => new
                {
                    TipoSetorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoSetores", x => x.TipoSetorId);
                });

            migrationBuilder.InsertData(
                table: "TipoSetores",
                columns: new[] { "TipoSetorId", "Descricao" },
                values: new object[,]
                {
                    { 1, "Ambulatório" },
                    { 2, "Administrativo" },
                    { 3, "Exame" },
                    { 4, "Externo" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoSetores");
        }
    }
}
