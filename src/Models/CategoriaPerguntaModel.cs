namespace TestesFOILMinimalApi.Models;

public class CategoriaPerguntaModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Abreviacao { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool? ValeUm { get; set; }

    public virtual ICollection<PerguntaModel> Perguntas { get; set; }
}
