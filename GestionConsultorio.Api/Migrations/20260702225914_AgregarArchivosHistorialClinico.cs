using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionConsultorio.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarArchivosHistorialClinico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HistorialesClinicos_TurnoId",
                table: "HistorialesClinicos");

            migrationBuilder.CreateTable(
                name: "ArchivosHistorialClinico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistorialClinicoId = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoContenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TamanioBytes = table.Column<long>(type: "bigint", nullable: false),
                    FechaCarga = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosHistorialClinico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivosHistorialClinico_HistorialesClinicos_HistorialClinicoId",
                        column: x => x.HistorialClinicoId,
                        principalTable: "HistorialesClinicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesClinicos_TurnoId",
                table: "HistorialesClinicos",
                column: "TurnoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosHistorialClinico_HistorialClinicoId",
                table: "ArchivosHistorialClinico",
                column: "HistorialClinicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivosHistorialClinico");

            migrationBuilder.DropIndex(
                name: "IX_HistorialesClinicos_TurnoId",
                table: "HistorialesClinicos");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialesClinicos_TurnoId",
                table: "HistorialesClinicos",
                column: "TurnoId");
        }
    }
}
