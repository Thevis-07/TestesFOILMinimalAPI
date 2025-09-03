using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestesFOILMinimalApi.Models;

namespace TestesFOILMinimalApi.Configurations
{
    public class ResultadoConfiguration : IEntityTypeConfiguration<ResultadoModel>
    {
        public void Configure(EntityTypeBuilder<ResultadoModel> b)
        {
            b.ToTable("resultado");

            b.HasKey(r => r.Id);

            b.Property(r => r.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            b.Property(r => r.TotalMae)
                .HasColumnName("total_mae")
                .IsRequired();

            b.Property(r => r.TotalPai)
                .HasColumnName("total_pai")
                .IsRequired();

            b.Property(r => r.AlunoId)
                .HasColumnName("aluno_id")
                .IsRequired();

            b.Property(r => r.CategoriaId)
                .HasColumnName("categoria_id")
                .IsRequired();

            b.Property(r => r.CriadoEm)
                .HasColumnName("criado_em")
                .IsRequired();

            b.HasOne(r => r.Aluno)
                .WithMany(a => a.Resultados)
                .HasForeignKey(r => r.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(r => r.Categoria)
                .WithMany()
                .HasForeignKey(r => r.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
