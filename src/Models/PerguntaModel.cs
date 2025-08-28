namespace TestesFOILMinimalApi.Models;

public class PerguntaModel
{
    public Guid Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int CategoriaPerguntaId { get; set; }
    public CategoriaPerguntaModel CategoriaPergunta { get; set; } = new CategoriaPerguntaModel();
}
