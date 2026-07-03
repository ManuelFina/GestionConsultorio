using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionConsultorio.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarResourceTypeArchivoHistorialv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "ArchivosHistorialClinico",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "ArchivosHistorialClinico");
        }
    }
}
