using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionConsultorio.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarBajaLogicaPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Pacientes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                table: "Pacientes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                table: "Pacientes");
        }
    }
}
