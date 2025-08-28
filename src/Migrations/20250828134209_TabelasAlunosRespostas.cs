using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestesFOILMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class TabelasAlunosRespostas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pergunta",
                newName: "id");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "pergunta",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "aluno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Idade = table.Column<int>(type: "integer", nullable: false),
                    Curso = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Semestre = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aluno", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "resposta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PerguntaId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlunoId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValorMae = table.Column<int>(type: "integer", nullable: false),
                    ValorPai = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resposta_aluno_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "aluno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resposta_pergunta_PerguntaId",
                        column: x => x.PerguntaId,
                        principalTable: "pergunta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resultado",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    AlunoId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: false),
                    TotalMae = table.Column<int>(type: "integer", nullable: false),
                    TotalPai = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resultado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resultado_aluno_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "aluno",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resultado_categorias_perguntas_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "categorias_perguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_resposta_AlunoId",
                table: "resposta",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_PerguntaId",
                table: "resposta",
                column: "PerguntaId");

            migrationBuilder.CreateIndex(
                name: "IX_resultado_AlunoId",
                table: "resultado",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_resultado_CategoriaId",
                table: "resultado",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "resposta");

            migrationBuilder.DropTable(
                name: "resultado");

            migrationBuilder.DropTable(
                name: "aluno");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "pergunta",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "pergunta",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}
