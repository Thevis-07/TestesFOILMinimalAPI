namespace TestesFOILMinimalApi.Dtos
{
    public class PerguntasDtos
    {
        public record PerguntaCreateDto(string Texto, int CategoriaPerguntaId);
        public record PerguntaUpdateDto(string Texto, int CategoriaPerguntaId);
        public record PerguntaDto(Guid Id, string Texto, int CategoriaPerguntaId, string? CategoriaNome);
    }
}
