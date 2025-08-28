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
    
        b.Property(p => p.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        b.Property(p => p.Texto)
            .HasColumnName("texto")
            .IsRequired()
            .HasColumnType("text");


        b.Property(p => p.CategoriaPerguntaId)
            .IsRequired()
            .HasColumnName("categoria_pergunta_id");

        b.Property(p => p.Ordem)
            .HasColumnName("ordem")
           .IsRequired()
           .UseIdentityByDefaultColumn();

        b.HasOne(p => p.CategoriaPergunta)
            .WithMany(c => c.Perguntas) 
            .HasForeignKey(p => p.CategoriaPerguntaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
