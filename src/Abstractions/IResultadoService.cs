using System;
using System.Threading.Tasks;
using TestesFOILMinimalApi.Models;
using static TestesFOILMinimalApi.Dtos.ResultadosDto;

namespace TestesFOILMinimalApi.Abstractions
{
    public interface IResultadoService
    {
        Task<IEnumerable<ResultadoReadDto>> CalcularResultadoAsync(Guid alunoId);
    }
}