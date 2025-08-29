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

        public record RespostaCreateListDto(
            Guid AlunoId,IEnumerable<RespostaItem> Respostas
        );

        public record RespostaItem(
            Guid PerguntaId,
            int ValorMae,
            int ValorPai
        );

        public record RespostaReadDto(
            Guid Id,
            string Aluno,
            string Pergunta,
            int ValorMae,
            int ValorPai
        );
    }
}
