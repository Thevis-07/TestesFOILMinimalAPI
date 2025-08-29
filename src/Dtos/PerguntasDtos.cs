namespace TestesFOILMinimalApi.Dtos
{
    public class PerguntasDtos
    {
        public record PerguntaCreateDto(string Texto, Guid CategoriaPerguntaId);
        public record PerguntaUpdateDto(string Texto, Guid CategoriaPerguntaId);
        public record PerguntaDto(Guid Id, string Texto, Guid CategoriaPerguntaId, string? CategoriaNome);
    }
}
