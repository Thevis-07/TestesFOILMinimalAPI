namespace TestesFOILMinimalApi.Dtos
{
    public class RespostasDto
    {
        public record RespostaCreateDto(
            Guid AlunoId,
            Guid PerguntaId,
            int ValorMae,
            int ValorPai
        );
    }
}
