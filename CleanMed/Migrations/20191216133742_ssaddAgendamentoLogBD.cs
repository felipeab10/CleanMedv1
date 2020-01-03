using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanMed.Migrations
{
    public partial class ssaddAgendamentoLogBD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendamentoLogs_Usuarios_UsuarioId1",
                table: "AgendamentoLogs");

            migrationBuilder.DropIndex(
                name: "IX_AgendamentoLogs_UsuarioId1",
                table: "AgendamentoLogs");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "AgendamentoLogs");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "AgendamentoLogs",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoLogs_UsuarioId",
                table: "AgendamentoLogs",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendamentoLogs_Usuarios_UsuarioId",
                table: "AgendamentoLogs",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendamentoLogs_Usuarios_UsuarioId",
                table: "AgendamentoLogs");

            migrationBuilder.DropIndex(
                name: "IX_AgendamentoLogs_UsuarioId",
                table: "AgendamentoLogs");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "AgendamentoLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "AgendamentoLogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoLogs_UsuarioId1",
                table: "AgendamentoLogs",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendamentoLogs_Usuarios_UsuarioId1",
                table: "AgendamentoLogs",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
