using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestesFOILMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aluno",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    idade = table.Column<int>(type: "integer", nullable: false),
                    curso = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    semestre = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aluno", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categorias_perguntas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    vale_um = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias_perguntas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pergunta",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    texto = table.Column<string>(type: "text", nullable: false),
                    categoria_pergunta_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ordem = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pergunta", x => x.id);
                    table.ForeignKey(
                        name: "FK_pergunta_categorias_perguntas_categoria_pergunta_id",
                        column: x => x.categoria_pergunta_id,
                        principalTable: "categorias_perguntas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "resultado",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    aluno_id = table.Column<Guid>(type: "uuid", nullable: false),
                    categoria_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_mae = table.Column<int>(type: "integer", nullable: false),
                    total_pai = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resultado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resultado_aluno_aluno_id",
                        column: x => x.aluno_id,
                        principalTable: "aluno",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resultado_categorias_perguntas_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "categorias_perguntas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resposta",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    pergunta_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aluno_id = table.Column<Guid>(type: "uuid", nullable: false),
                    valor_mae = table.Column<int>(type: "integer", nullable: false),
                    valor_pai = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resposta", x => x.id);
                    table.ForeignKey(
                        name: "FK_resposta_aluno_aluno_id",
                        column: x => x.aluno_id,
                        principalTable: "aluno",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resposta_pergunta_pergunta_id",
                        column: x => x.pergunta_id,
                        principalTable: "pergunta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pergunta_categoria_pergunta_id",
                table: "pergunta",
                column: "categoria_pergunta_id");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_aluno_id",
                table: "resposta",
                column: "aluno_id");

            migrationBuilder.CreateIndex(
                name: "IX_resposta_pergunta_id",
                table: "resposta",
                column: "pergunta_id");

            migrationBuilder.CreateIndex(
                name: "IX_resultado_aluno_id",
                table: "resultado",
                column: "aluno_id");

            migrationBuilder.CreateIndex(
                name: "IX_resultado_categoria_id",
                table: "resultado",
                column: "categoria_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "resposta");

            migrationBuilder.DropTable(
                name: "resultado");

            migrationBuilder.DropTable(
                name: "pergunta");

            migrationBuilder.DropTable(
                name: "aluno");

            migrationBuilder.DropTable(
                name: "categorias_perguntas");
        }
    }
}
