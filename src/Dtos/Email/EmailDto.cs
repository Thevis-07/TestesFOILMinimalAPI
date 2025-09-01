namespace TestesFOILMinimalApi.Dtos.Email
{
    public class EmailDto
    {
        public record CategoriaResumoItem(
            Guid Id,
            string Aluno,
            string CategoriaNome,
            string CategoriaAbreviacao,
            string CategoriaDescricao,
            int TotalMae,
            int TotalPai
        );

        public record AlunoEmail(
            string nome,
            string? curso = null,
            string? idade = null,
            string? semestre = null
        );

        public record EmailAluno(
            AlunoEmail aluno,
            List<CategoriaResumoItem> itens
        );
    }
}
