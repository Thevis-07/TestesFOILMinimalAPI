using TestesFOILMinimalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestesFOILMinimalApi.Configurations;

public sealed class PerguntaConfiguration : IEntityTypeConfiguration<PerguntaModel>
{
    public void Configure(EntityTypeBuilder<PerguntaModel> b)
    {
        b.ToTable("pergunta");

        b.HasKey(p => p.Id);

        // Se você tiver a extensão pgcrypto/uuid-ossp habilitada,
        // pode deixar o Id gerar no banco:
        // b.Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()");
        b.Property(p => p.Id).ValueGeneratedNever(); // ou gere Guid no app

        b.Property(p => p.Texto)
            .IsRequired()
            .HasMaxLength(1000);

        b.Property(p => p.CategoriaPerguntaId)
            .IsRequired()
            .HasColumnName("categoria_pergunta_id");

        b.HasOne(p => p.CategoriaPergunta)
            .WithMany(c => c.Perguntas) // se não tiver a coleção, use .WithMany()
            .HasForeignKey(p => p.CategoriaPerguntaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
