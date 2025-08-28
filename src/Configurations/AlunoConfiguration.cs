using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestesFOILMinimalApi.Models;

namespace TestesFOILMinimalApi.Configurations
{
    public class AlunoConfiguration : IEntityTypeConfiguration<AlunoModel>
    {
        public void Configure(EntityTypeBuilder<AlunoModel> b)
        {
            b.ToTable("aluno");

            b.HasKey(a => a.Id);

            b.Property(a => a.Id)
                .HasColumnName("id")    
                .HasDefaultValueSql("gen_random_uuid()");

            b.Property(a => a.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(150);

            b.Property(a => a.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(200);

            b.Property(a => a.Curso)
                .HasColumnName("curso")
                .IsRequired()
                .HasMaxLength(150);

            b.Property(a => a.Semestre)
                .HasColumnName("semestre")
                .IsRequired();

            b.Property(a => a.Idade)
                .HasColumnName("idade")
                .IsRequired();
        }
    }
}
