using Microsoft.EntityFrameworkCore;
using ConectorAnaliticoApi.Models;

namespace ConectorAnaliticoApi.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PerguntaModel> Perguntas => Set<PerguntaModel>();
    public DbSet<CategoriaPerguntaModel> Categorias => Set<CategoriaPerguntaModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
