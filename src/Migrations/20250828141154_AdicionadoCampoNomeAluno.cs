using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestesFOILMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoCampoNomeAluno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_resultado_aluno_AlunoId",
                table: "resultado");

            migrationBuilder.DropForeignKey(
                name: "FK_resultado_categorias_perguntas_CategoriaId",
                table: "resultado");

            migrationBuilder.RenameColumn(
                name: "TotalPai",
                table: "resultado",
                newName: "total_pai");

            migrationBuilder.RenameColumn(
                name: "TotalMae",
                table: "resultado",
                newName: "total_mae");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "resultado",
                newName: "categoria_id");

            migrationBuilder.RenameColumn(
                name: "AlunoId",
                table: "resultado",
                newName: "aluno_id");

            migrationBuilder.RenameIndex(
                name: "IX_resultado_CategoriaId",
                table: "resultado",
                newName: "IX_resultado_categoria_id");

            migrationBuilder.RenameIndex(
                name: "IX_resultado_AlunoId",
                table: "resultado",
                newName: "IX_resultado_aluno_id");

            migrationBuilder.RenameColumn(
                name: "Ordem",
                table: "pergunta",
                newName: "ordem");

            migrationBuilder.AddColumn<string>(
                name: "nome",
                table: "aluno",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_resultado_aluno_aluno_id",
                table: "resultado",
                column: "aluno_id",
                principalTable: "aluno",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_resultado_categorias_perguntas_categoria_id",
                table: "resultado",
                column: "categoria_id",
                principalTable: "categorias_perguntas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_resultado_aluno_aluno_id",
                table: "resultado");

            migrationBuilder.DropForeignKey(
                name: "FK_resultado_categorias_perguntas_categoria_id",
                table: "resultado");

            migrationBuilder.DropColumn(
                name: "nome",
                table: "aluno");

            migrationBuilder.RenameColumn(
                name: "total_pai",
                table: "resultado",
                newName: "TotalPai");

            migrationBuilder.RenameColumn(
                name: "total_mae",
                table: "resultado",
                newName: "TotalMae");

            migrationBuilder.RenameColumn(
                name: "categoria_id",
                table: "resultado",
                newName: "CategoriaId");

            migrationBuilder.RenameColumn(
                name: "aluno_id",
                table: "resultado",
                newName: "AlunoId");

            migrationBuilder.RenameIndex(
                name: "IX_resultado_categoria_id",
                table: "resultado",
                newName: "IX_resultado_CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_resultado_aluno_id",
                table: "resultado",
                newName: "IX_resultado_AlunoId");

            migrationBuilder.RenameColumn(
                name: "ordem",
                table: "pergunta",
                newName: "Ordem");

            migrationBuilder.AddForeignKey(
                name: "FK_resultado_aluno_AlunoId",
                table: "resultado",
                column: "AlunoId",
                principalTable: "aluno",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_resultado_categorias_perguntas_CategoriaId",
                table: "resultado",
                column: "CategoriaId",
                principalTable: "categorias_perguntas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
