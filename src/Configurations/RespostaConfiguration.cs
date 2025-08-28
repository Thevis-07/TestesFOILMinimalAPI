using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestesFOILMinimalApi.Models;

namespace TestesFOILMinimalApi.Configurations
{
    public class RespostaConfiguration : IEntityTypeConfiguration<RespostaModel>
    {
        public void Configure(EntityTypeBuilder<RespostaModel> b)
        {
            b.ToTable("resposta");

            b.HasKey(r => r.Id);

            b.Property(r => r.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            b.Property(r => r.ValorMae)
                .HasColumnName("valor_mae")
                .IsRequired();

            b.Property(r => r.ValorPai)
                .HasColumnName("valor_pai")
                .IsRequired();

            b.Property(r => r.PerguntaId)
                .HasColumnName("pergunta_id")
                .IsRequired();

            b.Property(r => r.AlunoId)
                .HasColumnName("aluno_id")
                .IsRequired();

            b.HasOne(r => r.Pergunta)
                .WithMany() 
                .HasForeignKey(r => r.PerguntaId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(r => r.Aluno)
                .WithMany(a => a.Respostas)
                .HasForeignKey(r => r.AlunoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
