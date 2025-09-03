using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestesFOILMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoCampoTabelaResultado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "criado_em",
                table: "resultado",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "criado_em",
                table: "resultado");
        }
    }
}
