using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Models;
using static TestesFOILMinimalApi.Dtos.AlunosDto;

namespace TestesFOILMinimalApi.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _db;

        public AlunoService(AppDbContext db) => _db = db;

        public async Task<AlunoModel> CreateAsync(AlunoCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email é obrigatório");

            var aluno = new AlunoModel
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim(),
                Idade = dto.Idade,
                Curso = dto.Curso.Trim(),
                Semestre = dto.Semestre
            };

            _db.Alunos.Add(aluno);
            await _db.SaveChangesAsync();

            return aluno;
        }

        public async Task<AlunoModel?> GetByIdAsync(Guid id) =>
            await _db.Alunos.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IReadOnlyList<AlunoModel>> ListAsync() =>
            await _db.Alunos.AsNoTracking().ToListAsync();

        public async Task<bool> UpdateAsync(Guid id, AlunoUpdateDto dto)
        {
            var aluno = await _db.Alunos.FirstOrDefaultAsync(a => a.Id == id);
            if (aluno is null) return false;


            aluno.Email = dto.Email.Trim();
            aluno.Idade = dto.Idade;
            aluno.Curso = dto.Curso.Trim();
            aluno.Semestre = dto.Semestre;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var aluno = await _db.Alunos.FirstOrDefaultAsync(a => a.Id == id);
            if (aluno is null) return false;

            _db.Alunos.Remove(aluno);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
