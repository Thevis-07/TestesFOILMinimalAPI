using static TestesFOILMinimalApi.Dtos.AlunosDto;

namespace TestesFOILMinimalApi.Dtos
{
    public class ResultadosDto
    {
        public record ResultadoReadDto(
            AlunoReadDto aluno, IEnumerable<ResultadoItemDto> resultado
        );

        public record ResultadoItemDto(
            Guid CategoriaId,
            string CategoriaNome,
            string CategoriaAbreviacao,
            string CategoriaDescricao,
            int TotalMae,
            int TotalPai);
    }
}
