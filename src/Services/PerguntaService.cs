using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Abstractions;
using TestesFOILMinimalApi.Data;
using TestesFOILMinimalApi.Models;
using static TestesFOILMinimalApi.Dtos.PerguntasDtos;

namespace TestesFOILMinimalApi.Services
{
    public class PerguntaService(AppDbContext db) : IPerguntaService
    {
        public async Task<PerguntaDto> CreateAsync(PerguntaCreateDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Texto))
                throw new ArgumentException("Texto é obrigatório.");

            var catExists = await db.Categorias.AnyAsync(c => c.Id == dto.CategoriaPerguntaId);
            if (!catExists) throw new ArgumentException("Categoria inválida.");

            var entity = new PerguntaModel
            {
                Id = Guid.NewGuid(),
                Texto = dto.Texto.Trim(),
                CategoriaPerguntaId = dto.CategoriaPerguntaId
            };

            db.Perguntas.Add(entity);
            await db.SaveChangesAsync();

            return new PerguntaDto(entity.Id, entity.Texto, entity.CategoriaPerguntaId, null);
        }

        public async Task<List<PerguntaDto>> ListAsync() =>
            await db.Perguntas.AsNoTracking()
                .Include(p => p.CategoriaPergunta)
                .Select(p => new PerguntaDto(p.Id, p.Texto, p.CategoriaPerguntaId, p.CategoriaPergunta.Nome))
                .ToListAsync();

        public async Task<PerguntaDto?> GetAsync(Guid id) =>
            await db.Perguntas.AsNoTracking()
                .Include(p => p.CategoriaPergunta)
                .Where(p => p.Id == id)
                .Select(p => new PerguntaDto(p.Id, p.Texto, p.CategoriaPerguntaId, p.CategoriaPergunta.Nome))
                .FirstOrDefaultAsync();

        public async Task<bool> UpdateAsync(Guid id, PerguntaUpdateDto dto)
        {
            var e = await db.Perguntas.FirstOrDefaultAsync(p => p.Id == id);
            if (e is null) return false;

            var catExists = await db.Categorias.AnyAsync(c => c.Id == dto.CategoriaPerguntaId);
            if (!catExists) throw new ArgumentException("Categoria inválida.");

            e.Texto = dto.Texto.Trim();
            e.CategoriaPerguntaId = dto.CategoriaPerguntaId;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var e = await db.Perguntas.FirstOrDefaultAsync(p => p.Id == id);
            if (e is null) return false;
            db.Perguntas.Remove(e);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
