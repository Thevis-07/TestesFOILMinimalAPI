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
                .AsNoTracking()
                .Include(r => r.Aluno)
                .Include(r => r.Pergunta).ThenInclude(p => p.CategoriaPergunta)
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

            var categoriasDasRespostas = respostas
                .Select(r => r.Pergunta.CategoriaPerguntaId)
                .Distinct()
                .ToList();

            var existentes = await _db.Resultados
                .AsNoTracking()
                .Where(r => r.AlunoId == alunoId)
                .ToListAsync();

            bool existeResultadoCompleto =
                existentes.Any() &&
                categoriasDasRespostas.All(cid => existentes.Any(e => e.CategoriaId == cid));

            if (existeResultadoCompleto)
            {
                var categorias = await _db.Categorias
                    .AsNoTracking()
                    .Where(c => categoriasDasRespostas.Contains(c.Id))
                    .ToDictionaryAsync(c => c.Id);

                var resultadoExistente = existentes
                    .Where(e => categorias.ContainsKey(e.CategoriaId))
                    .Select(e =>
                    {
                        var c = categorias[e.CategoriaId];
                        return new ResultadoItemDto(
                            CategoriaId: c.Id,
                            CategoriaNome: c.Nome,
                            CategoriaAbreviacao: c.Abreviacao,
                            CategoriaDescricao: c.Descricao,
                            TotalMae: e.TotalMae,
                            TotalPai: e.TotalPai
                        );
                    })
                    .OrderByDescending(x => x.TotalPai + x.TotalMae)
                    .ToList();

                return new ResultadoReadDto(alunoDto, resultadoExistente);
            }


            var resultadoCalculado = respostas
                .GroupBy(r => new
                {
                    r.Pergunta.CategoriaPerguntaId,
                    r.Pergunta.CategoriaPergunta.Nome,
                    r.Pergunta.CategoriaPergunta.Abreviacao,
                    r.Pergunta.CategoriaPergunta.Descricao,
                    ValeUm = r.Pergunta.CategoriaPergunta.ValeUm
                })
                .Select(g => new ResultadoItemDto(
                    CategoriaId: g.Key.CategoriaPerguntaId,
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

            // Remove antigos do aluno e insere os novos
            var antigos = _db.Resultados.Where(r => r.AlunoId == alunoId);
            _db.Resultados.RemoveRange(antigos);
            _db.Resultados.AddRange(resultadoCalculado.Select(r => new Models.ResultadoModel
            {
                Id = Guid.NewGuid(),
                AlunoId = alunoId,
                CategoriaId = r.CategoriaId,
                TotalMae = r.TotalMae,
                TotalPai = r.TotalPai
            }));

            await _db.SaveChangesAsync();

            var alunoEmail = new EmailDto.AlunoEmail(
                nome: aluno.Nome,
                curso: aluno.Curso,
                idade: aluno.Idade.ToString(),
                semestre: aluno.Semestre.ToString()
            );

            var itensEmail = resultadoCalculado.Select(i => new EmailDto.CategoriaResumoItem(
                Id: Guid.NewGuid(),
                Aluno: aluno.Nome,
                CategoriaNome: i.CategoriaNome,
                CategoriaAbreviacao: i.CategoriaAbreviacao,
                CategoriaDescricao: i.CategoriaDescricao,
                TotalMae: i.TotalMae,
                TotalPai: i.TotalPai
            )).ToList();

            // Envia e-mail APENAS quando recalculamos/persistimos
            await _email.SendRelatorioCategoriasAsync(
                itens: itensEmail,
                to: aluno.Email,
                aluno: alunoEmail,
                subjectPrefix: "Resultados Estilos Parentais"
            );

            return new ResultadoReadDto(alunoDto, resultadoCalculado);
        }

    }
}
