using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestesFOILMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoCampoDescricao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "descricao",
                table: "categorias_perguntas",
                type: "character varying(1500)",
                maxLength: 1500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "descricao",
                table: "categorias_perguntas");
        }
    }
}
