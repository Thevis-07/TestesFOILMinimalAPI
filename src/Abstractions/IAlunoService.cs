using TestesFOILMinimalApi.Models;
using static TestesFOILMinimalApi.Dtos.AlunosDto;

namespace TestesFOILMinimalApi.Abstractions
{
    public interface IAlunoService
    {
        Task<AlunoModel> CreateAsync(AlunoCreateDto dto);
        Task<AlunoModel?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<AlunoModel>> ListAsync();
        Task<bool> UpdateAsync(Guid id, AlunoUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
