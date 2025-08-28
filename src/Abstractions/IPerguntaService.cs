using static TestesFOILMinimalApi.Dtos.PerguntasDtos;

namespace TestesFOILMinimalApi.Abstractions
{
    public interface IPerguntaService
    {
        Task<PerguntaDto> CreateAsync(PerguntaCreateDto dto);
        Task<List<PerguntaDto>> ListAsync();
        Task<bool> UpdateAsync(Guid id, PerguntaUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
