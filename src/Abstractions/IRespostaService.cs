using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Models;
using TestesFOILMinimalApi.Services;
using static TestesFOILMinimalApi.Dtos.RespostasDto;

namespace TestesFOILMinimalApi.Abstractions
{
    public interface IRespostaService
    {
        Task<RespostaModel> SaveAsync(RespostaCreateDto input);
        Task<IReadOnlyList<RespostaModel>> SaveManyAsync(RespostaCreateListDto inputs);
    }
}
