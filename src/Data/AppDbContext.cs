using Microsoft.EntityFrameworkCore;
using TestesFOILMinimalApi.Models;

namespace TestesFOILMinimalApi.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PerguntaModel> Perguntas => Set<PerguntaModel>();
    public DbSet<CategoriaPerguntaModel> Categorias => Set<CategoriaPerguntaModel>();
    public DbSet<RespostaModel> Respostas => Set<RespostaModel>();
    public DbSet<ResultadoModel> Resultados => Set<ResultadoModel>();
    public DbSet<AlunoModel> Alunos => Set<AlunoModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
