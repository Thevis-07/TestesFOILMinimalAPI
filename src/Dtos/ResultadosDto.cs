namespace TestesFOILMinimalApi.Dtos
{
    public class ResultadosDto
    {
        public record ResultadoReadDto(
            Guid Id,
            string Aluno,
            string CategoriaNome,
            string CategoriaAbreviacao,
            int TotalMae,
            int TotalPai
        );
    }
}
