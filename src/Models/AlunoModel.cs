namespace TestesFOILMinimalApi.Models
{
    public class AlunoModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Idade { get; set; }
        public string Curso { get; set; } = string.Empty;
        public int Semestre { get; set; }

        // Relação
        public ICollection<RespostaModel> Respostas { get; set; } = new List<RespostaModel>();
        public ICollection<ResultadoModel> Resultados { get; set; } = new List<ResultadoModel>();
    }
}
