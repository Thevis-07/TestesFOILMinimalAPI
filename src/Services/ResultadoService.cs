using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Dtos;
using TestesFOILMinimalApi.Dtos.Email;
using static TestesFOILMinimalApi.Dtos.ResultadosDto;

namespace TestesFOILMinimalApi.Services
{
    public class ResultadoService : IResultadoService
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _email;
        public ResultadoService(AppDbContext db, IEmailService email)
        {
            _db = db;
            _email = email;
        }

        public async Task<ResultadoReadDto> CalcularResultadoAsync(Guid alunoId)
        {
            var respostas = await _db.Respostas
                .Include(r => r.Aluno)
                .Include(r => r.Pergunta)
                .ThenInclude(p => p.CategoriaPergunta)
                .Where(r => r.AlunoId == alunoId)
                .ToListAsync();

            if (!respostas.Any())
                return null;

            var aluno = respostas.First().Aluno;
            var alunoDto = new AlunosDto.AlunoReadDto(
                Id: aluno.Id,
                Nome: aluno.Nome,
                Email: aluno.Email,
                Idade: aluno.Idade,
                Curso: aluno.Curso,
                Semestre: aluno.Semestre
            );

            var resultado = respostas
                .GroupBy(r => new
                {
                    r.Pergunta.CategoriaPerguntaId,
                    r.Pergunta.CategoriaPergunta.Nome,
                    r.Pergunta.CategoriaPergunta.Abreviacao,
                    r.Pergunta.CategoriaPergunta.Descricao,
                    ValeUm = r.Pergunta.CategoriaPergunta.ValeUm
                })
                .Select(g => new ResultadoItemDto(
                    CategoriaNome: g.Key.Nome,
                    CategoriaAbreviacao: g.Key.Abreviacao,
                    CategoriaDescricao: g.Key.Descricao,
                    TotalMae: g.Key.ValeUm == true
                        ? g.Count(r => r.ValorMae == 1 || r.ValorMae == 2)
                        : g.Count(r => r.ValorMae > 4),
                    TotalPai: g.Key.ValeUm == true
                        ? g.Count(r => r.ValorPai == 1 || r.ValorPai == 2)
                        : g.Count(r => r.ValorPai > 4)
                ))
                .OrderByDescending(x => x.TotalPai + x.TotalMae)
                .ToList();

            // remove anteriores e salva os novos
            var antigos = _db.Resultados.Where(r => r.AlunoId == alunoId);
            _db.Resultados.RemoveRange(antigos);
            await _db.SaveChangesAsync();

            // Mapeia para os records usados no envio de e-mail
            var alunoEmail = new EmailDto.AlunoEmail(
                nome: aluno.Nome,
                curso: aluno.Curso,
                idade: aluno.Idade.ToString(),
                semestre: aluno.Semestre.ToString()
            );

            var itensEmail = resultado.Select(i => new EmailDto.CategoriaResumoItem(
                Id: Guid.NewGuid(), // ou use um id relevante se necessário
                Aluno: aluno.Nome,
                CategoriaNome: i.CategoriaNome,
                CategoriaAbreviacao: i.CategoriaAbreviacao,
                TotalMae: i.TotalMae,
                TotalPai: i.TotalPai
            )).ToList();

            var _ =  _email.SendRelatorioCategoriasAsync(
                itens: itensEmail,
                to: aluno.Email,
                aluno: alunoEmail,
                subjectPrefix: "Resultados Estilos Parentais"
            );

            return new ResultadoReadDto(alunoDto, resultado);
        }
    }
}
