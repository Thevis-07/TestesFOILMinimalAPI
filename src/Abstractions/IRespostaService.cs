using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Dtos;
using TestesFOILMinimalApi.Models;
using TestesFOILMinimalApi.Services;
using static TestesFOILMinimalApi.Dtos.RespostasDto;

namespace TestesFOILMinimalApi.Abstractions
{
    public interface IRespostaService
    {
        Task<RespostaModel> SaveAsync(RespostaCreateDto input);
        Task<IEnumerable<RespostaReadDto>> SaveManyAsync(RespostaCreateListDto inputs);
        Task<IEnumerable<RespostaDetalhadaDto>> ListDetalhadaAsync(Guid alunoId);
    }
}
