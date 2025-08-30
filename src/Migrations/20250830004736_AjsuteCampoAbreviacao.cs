using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestesFOILMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AjsuteCampoAbreviacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Abreviacao",
                table: "categorias_perguntas",
                newName: "abreviacao");

            migrationBuilder.AlterColumn<string>(
                name: "abreviacao",
                table: "categorias_perguntas",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                defaultValue: "False",
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "abreviacao",
                table: "categorias_perguntas",
                newName: "Abreviacao");

            migrationBuilder.AlterColumn<string>(
                name: "Abreviacao",
                table: "categorias_perguntas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldDefaultValue: "False");
        }
    }
}
