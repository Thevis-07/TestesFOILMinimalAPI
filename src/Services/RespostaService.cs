using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Dtos;
using TestesFOILMinimalApi.Models;
using static TestesFOILMinimalApi.Dtos.RespostasDto;

namespace TestesFOILMinimalApi.Services;

public class RespostaService : IRespostaService
{
    private readonly AppDbContext _db;

    public RespostaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RespostaModel> SaveAsync(RespostaCreateDto input)
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

        return resp;
    }

    public async Task<IEnumerable<RespostaReadDto>> SaveManyAsync(RespostaCreateListDto dto)
    {
        var list = dto.Respostas.ToList();
        if (list.Count == 0) return Array.Empty<RespostasDto.RespostaReadDto>();

        var aluno = await _db.Alunos.FirstOrDefaultAsync(a => a.Id == dto.AlunoId);
        if (aluno is null) throw new ArgumentException("Aluno inexistente.");

        var perguntaIds = list.Select(i => i.PerguntaId).Distinct().ToArray();
        var perguntas = await _db.Perguntas
            .Where(p => perguntaIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var existentes = await _db.Respostas
            .Where(r => r.AlunoId == dto.AlunoId && perguntaIds.Contains(r.PerguntaId))
            .ToListAsync();

        var resultado = new List<RespostaModel>(list.Count);

        foreach (var i in list)
        {
            if (i.ValorMae is < 0 or > 10 || i.ValorPai is < 0 or > 10)
                throw new ArgumentOutOfRangeException("Valores devem estar entre 0 e 10.");

            if (!perguntas.ContainsKey(i.PerguntaId))
                throw new ArgumentException($"Pergunta {i.PerguntaId} inexistente.");

            var existente = existentes.FirstOrDefault(r => r.PerguntaId == i.PerguntaId);
            if (existente is null)
            {
                var novo = new RespostaModel
                {
                    AlunoId = dto.AlunoId,
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

        var respostaDtos = resultado.Select(r => new RespostasDto.RespostaReadDto(
            Id: r.Id,
            Aluno: aluno.Nome,
            Pergunta: perguntas[r.PerguntaId].Texto,
            ValorMae: r.ValorMae,
            ValorPai: r.ValorPai
        )).ToList();

        return respostaDtos;
    }

    public async Task<IEnumerable<RespostaDetalhadaDto>> ListDetalhadaAsync(Guid alunoId)
    {
        var respostas = await _db.Respostas
            .Where(r => r.AlunoId == alunoId)
            .Include(r => r.Aluno)
            .Include(r => r.Pergunta)
            .OrderBy(r => r.Pergunta.Texto)
            .ToListAsync();

        return respostas.Select(r => new RespostaDetalhadaDto(
            AlunoId: r.AlunoId,
            AlunoNome: r.Aluno.Nome,
            PerguntaId: r.PerguntaId,
            PerguntaNome: r.Pergunta.Texto,
            ValorMae: r.ValorMae,
            ValorPai: r.ValorPai
        ));
    }
}