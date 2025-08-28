using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestesFOILMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoNomesColunas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_resposta_aluno_AlunoId",
                table: "resposta");

            migrationBuilder.DropForeignKey(
                name: "FK_resposta_pergunta_PerguntaId",
                table: "resposta");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "resposta",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ValorPai",
                table: "resposta",
                newName: "valor_pai");

            migrationBuilder.RenameColumn(
                name: "ValorMae",
                table: "resposta",
                newName: "valor_mae");

            migrationBuilder.RenameColumn(
                name: "PerguntaId",
                table: "resposta",
                newName: "pergunta_id");

            migrationBuilder.RenameColumn(
                name: "AlunoId",
                table: "resposta",
                newName: "aluno_id");

            migrationBuilder.RenameIndex(
                name: "IX_resposta_PerguntaId",
                table: "resposta",
                newName: "IX_resposta_pergunta_id");

            migrationBuilder.RenameIndex(
                name: "IX_resposta_AlunoId",
                table: "resposta",
                newName: "IX_resposta_aluno_id");

            migrationBuilder.RenameColumn(
                name: "Texto",
                table: "pergunta",
                newName: "texto");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "categorias_perguntas",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "ValeUm",
                table: "categorias_perguntas",
                newName: "vale_um");

            migrationBuilder.RenameColumn(
                name: "Semestre",
                table: "aluno",
                newName: "semestre");

            migrationBuilder.RenameColumn(
                name: "Idade",
                table: "aluno",
                newName: "idade");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "aluno",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Curso",
                table: "aluno",
                newName: "curso");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "aluno",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_resposta_aluno_aluno_id",
                table: "resposta",
                column: "aluno_id",
                principalTable: "aluno",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_resposta_pergunta_pergunta_id",
                table: "resposta",
                column: "pergunta_id",
                principalTable: "pergunta",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_resposta_aluno_aluno_id",
                table: "resposta");

            migrationBuilder.DropForeignKey(
                name: "FK_resposta_pergunta_pergunta_id",
                table: "resposta");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "resposta",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "valor_pai",
                table: "resposta",
                newName: "ValorPai");

            migrationBuilder.RenameColumn(
                name: "valor_mae",
                table: "resposta",
                newName: "ValorMae");

            migrationBuilder.RenameColumn(
                name: "pergunta_id",
                table: "resposta",
                newName: "PerguntaId");

            migrationBuilder.RenameColumn(
                name: "aluno_id",
                table: "resposta",
                newName: "AlunoId");

            migrationBuilder.RenameIndex(
                name: "IX_resposta_pergunta_id",
                table: "resposta",
                newName: "IX_resposta_PerguntaId");

            migrationBuilder.RenameIndex(
                name: "IX_resposta_aluno_id",
                table: "resposta",
                newName: "IX_resposta_AlunoId");

            migrationBuilder.RenameColumn(
                name: "texto",
                table: "pergunta",
                newName: "Texto");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "categorias_perguntas",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "vale_um",
                table: "categorias_perguntas",
                newName: "ValeUm");

            migrationBuilder.RenameColumn(
                name: "semestre",
                table: "aluno",
                newName: "Semestre");

            migrationBuilder.RenameColumn(
                name: "idade",
                table: "aluno",
                newName: "Idade");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "aluno",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "curso",
                table: "aluno",
                newName: "Curso");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "aluno",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_resposta_aluno_AlunoId",
                table: "resposta",
                column: "AlunoId",
                principalTable: "aluno",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_resposta_pergunta_PerguntaId",
                table: "resposta",
                column: "PerguntaId",
                principalTable: "pergunta",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
