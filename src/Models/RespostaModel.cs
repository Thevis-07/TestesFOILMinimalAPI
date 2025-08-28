namespace TestesFOILMinimalApi.Models
{
    public class RespostaModel
    {
        public Guid Id { get; set; }

        public Guid PerguntaId { get; set; }
        public PerguntaModel Pergunta { get; set; } = null!;

        public Guid AlunoId { get; set; }
        public AlunoModel Aluno { get; set; } = null!;

        // valores da resposta
        public int ValorMae { get; set; }
        public int ValorPai { get; set; }
    }
}
