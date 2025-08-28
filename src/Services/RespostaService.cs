using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Models;
using static TestesFOILMinimalApi.Dtos.RespostasDto;

namespace TestesFOILMinimalApi.Services;

public class RespostaService : IRespostaService
{
    private readonly AppDbContext _db;
    private readonly ResultadoService _resultadoService; // se preferir interface, extraia IResultadoService

    public RespostaService(AppDbContext db, ResultadoService resultadoService)
    {
        _db = db;
        _resultadoService = resultadoService;
    }

    public async Task<RespostaModel> SaveAsync(RespostaCreateDto input, bool recalcResultado = false)
    {
        // validações básicas
        if (input.ValorMae < 0 || input.ValorPai < 0)
            throw new ArgumentException("Valores não podem ser negativos.");

        // (opcional) valida intervalo 0..10
        if (input.ValorMae is < 0 or > 10 || input.ValorPai is < 0 or > 10)
            throw new ArgumentOutOfRangeException("Valores devem estar entre 0 e 10.");

        // garante existência das FKs
        var alunoExists = await _db.Alunos.AnyAsync(a => a.Id == input.AlunoId);
        if (!alunoExists) throw new ArgumentException("Aluno inexistente.");

        var pergunta = await _db.Perguntas
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == input.PerguntaId);
        if (pergunta is null) throw new ArgumentException("Pergunta inexistente.");

        // upsert: se já existe resposta do aluno para essa pergunta, atualiza
        var resp = await _db.Respostas
            .FirstOrDefaultAsync(r => r.AlunoId == input.AlunoId && r.PerguntaId == input.PerguntaId);

        if (resp is null)
        {
            resp = new RespostaModel
            {
                Id = Guid.NewGuid(),
                AlunoId = input.AlunoId,
                PerguntaId = input.PerguntaId,
                ValorMae = input.ValorMae,
                ValorPai = input.ValorPai
            };
            _db.Respostas.Add(resp);
        }
        else
        {
            resp.ValorMae = input.ValorMae;
            resp.ValorPai = input.ValorPai;
        }

        await _db.SaveChangesAsync();

        if (recalcResultado)
            await _resultadoService.CalcularResultados(input.AlunoId);

        return resp;
    }

    public async Task<IReadOnlyList<RespostaModel>> SaveManyAsync(IEnumerable<RespostaCreateDto> inputs, bool recalcResultado = false)
    {
        var list = inputs.ToList();
        if (list.Count == 0) return Array.Empty<RespostaModel>();

        // valida FK aluno (assumindo todos do mesmo aluno no bulk)
        var alunoId = list[0].AlunoId;
        var alunoExists = await _db.Alunos.AnyAsync(a => a.Id == alunoId);
        if (!alunoExists) throw new ArgumentException("Aluno inexistente.");

        // carrega respostas existentes do aluno p/ upsert em memória
        var perguntaIds = list.Select(i => i.PerguntaId).Distinct().ToArray();
        var existentes = await _db.Respostas
            .Where(r => r.AlunoId == alunoId && perguntaIds.Contains(r.PerguntaId))
            .ToListAsync();

        var resultado = new List<RespostaModel>(list.Count);

        foreach (var i in list)
        {
            if (i.ValorMae is < 0 or > 10 || i.ValorPai is < 0 or > 10)
                throw new ArgumentOutOfRangeException("Valores devem estar entre 0 e 10.");

            // (opcional) validar existência da pergunta individualmente apenas se desejar
            var existente = existentes.FirstOrDefault(r => r.PerguntaId == i.PerguntaId);
            if (existente is null)
            {
                var novo = new RespostaModel
                {
                    Id = Guid.NewGuid(),
                    AlunoId = i.AlunoId,
                    PerguntaId = i.PerguntaId,
                    ValorMae = i.ValorMae,
                    ValorPai = i.ValorPai
                };
                _db.Respostas.Add(novo);
                resultado.Add(novo);
            }
            else
            {
                existente.ValorMae = i.ValorMae;
                existente.ValorPai = i.ValorPai;
                resultado.Add(existente);
            }
        }

        await _db.SaveChangesAsync();

        if (recalcResultado)
            await _resultadoService.CalcularResultados(alunoId);

        return resultado;
    }
}