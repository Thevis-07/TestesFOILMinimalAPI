using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Models;

namespace TestesFOILMinimalApi.Services
{
    public class ResultadoService
    {
        private readonly AppDbContext _db;
        public ResultadoService(AppDbContext db) => _db = db;

        public async Task CalcularResultados(Guid alunoId)
        {
            var respostas = await _db.Respostas
                .Include(r => r.Pergunta)
                .ThenInclude(p => p.CategoriaPergunta)
                .Where(r => r.AlunoId == alunoId)
                .ToListAsync();

            var resultados = respostas
                .GroupBy(r => r.Pergunta.CategoriaPerguntaId)
                .Select(g => new ResultadoModel
                {
                    Id = Guid.NewGuid(),
                    AlunoId = alunoId,
                    CategoriaId = g.Key,
                    TotalMae = g.Count(r => r.ValorMae > 5),
                    TotalPai = g.Count(r => r.ValorPai > 5)
                })
                .ToList();

            // remove anteriores e salva os novos
            var antigos = _db.Resultados.Where(r => r.AlunoId == alunoId);
            _db.Resultados.RemoveRange(antigos);
            _db.Resultados.AddRange(resultados);
            await _db.SaveChangesAsync();
        }
    }
}
