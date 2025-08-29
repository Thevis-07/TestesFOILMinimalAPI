namespace TestesFOILMinimalApi.Dtos
{
    public class ResultadosDto
    {
        public record ResultadoReadDto(
            Guid Id,
            string Aluno,
            Guid CategoriaId,
            string CategoriaNome,
            int TotalMae,
            int TotalPai
        );
    }
}
