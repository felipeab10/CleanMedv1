using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class UpdateSetorBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoSetor",
                table: "Setores");

            migrationBuilder.AddColumn<int>(
                name: "TipoSetorId",
                table: "Setores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Setores_TipoSetorId",
                table: "Setores",
                column: "TipoSetorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Setores_TipoSetores_TipoSetorId",
                table: "Setores",
                column: "TipoSetorId",
                principalTable: "TipoSetores",
                principalColumn: "TipoSetorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setores_TipoSetores_TipoSetorId",
                table: "Setores");

            migrationBuilder.DropIndex(
                name: "IX_Setores_TipoSetorId",
                table: "Setores");

            migrationBuilder.DropColumn(
                name: "TipoSetorId",
                table: "Setores");

            migrationBuilder.AddColumn<string>(
                name: "TipoSetor",
                table: "Setores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
