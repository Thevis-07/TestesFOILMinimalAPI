namespace TestesFOILMinimalApi.Models
{
    public class ResultadoModel
    {
        public Guid Id { get; set; }

        public Guid AlunoId { get; set; }
        public AlunoModel Aluno { get; set; } = null!;

        public int CategoriaId { get; set; }
        public CategoriaPerguntaModel Categoria { get; set; } = null!;

        public int TotalMae { get; set; }
        public int TotalPai { get; set; }
    }
}
