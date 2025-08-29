using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Models;
using static TestesFOILMinimalApi.Dtos.ResultadosDto;

namespace TestesFOILMinimalApi.Services
{
    public class ResultadoService : IResultadoService
    {
        private readonly AppDbContext _db;
        public ResultadoService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<ResultadoReadDto>> CalcularResultadoAsync(Guid alunoId)
        {
            var respostas = await _db.Respostas
                .Include(r => r.Aluno)
                .Include(r => r.Pergunta)
                .ThenInclude(p => p.CategoriaPergunta)
                .Where(r => r.AlunoId == alunoId)
                .ToListAsync();

            // Fix: Pass all required constructor parameters to ResultadoReadDto
            var resultados = respostas
                .GroupBy(r => new { r.Pergunta.CategoriaPerguntaId, r.Pergunta.CategoriaPergunta.Nome })
                .Select(g => new ResultadoReadDto(
                    Id: Guid.NewGuid(), // ou use um Id relevante
                    Aluno: g.First().Aluno.Nome, // pega o nome do aluno do primeiro item do grupo
                    CategoriaId: g.Key.CategoriaPerguntaId,
                    CategoriaNome: g.Key.Nome,
                    TotalMae: g.Count(r => r.ValorMae > 5),
                    TotalPai: g.Count(r => r.ValorPai > 5)
                ))
                .ToList();

            // remove anteriores e salva os novos
            var antigos = _db.Resultados.Where(r => r.AlunoId == alunoId);
            _db.Resultados.RemoveRange(antigos);
            await _db.SaveChangesAsync();

            return resultados;
        }
    }
}
