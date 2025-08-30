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

            var resultados = respostas
                    .GroupBy(r => new { r.Pergunta.CategoriaPerguntaId, r.Pergunta.CategoriaPergunta.Nome, ValeUm = r.Pergunta.CategoriaPergunta.ValeUm, Abreviacao = r.Pergunta.CategoriaPergunta.Abreviacao })
                    .Select(g => new ResultadoReadDto(
                        Id: Guid.NewGuid(),
                        Aluno: g.First().Aluno.Nome,
                        CategoriaNome: g.Key.Nome,
                        CategoriaAbreviacao: g.Key.Abreviacao,
                        TotalMae: g.Key.ValeUm == true
                            ? g.Count(r => r.ValorMae == 1 || r.ValorMae == 2)
                            : g.Count(r => r.ValorMae > 4),
                        TotalPai: g.Key.ValeUm == true
                            ? g.Count(r => r.ValorPai == 1 || r.ValorPai == 2)
                            : g.Count(r => r.ValorPai > 4)
                    ))
                    .ToList();

            // remove anteriores e salva os novos
            var antigos = _db.Resultados.Where(r => r.AlunoId == alunoId);
            _db.Resultados.RemoveRange(antigos);
            await _db.SaveChangesAsync();

            return resultados.OrderByDescending(x => x.TotalPai + x.TotalMae);
        }
    }
}
