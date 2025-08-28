namespace ConectorAnaliticoApi.Models;

public class CategoriaPerguntaModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool ValeUm { get; set; }

    public virtual ICollection<PerguntaModel> Perguntas { get; set; }
}
