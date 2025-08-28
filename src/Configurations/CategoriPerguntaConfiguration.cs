using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestesFOILMinimalApi.Models;

namespace TestesFOILMinimalApi.Configurations
{
    public class CategoriPerguntaConfiguration : IEntityTypeConfiguration<CategoriaPerguntaModel>
    {
        public void Configure(EntityTypeBuilder<CategoriaPerguntaModel> builder)
        {
            builder.ToTable("categorias_perguntas");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.ValeUm)
                .HasColumnName("vale_um")
                .IsRequired(false)
                .HasDefaultValue(false);
        }
    }
}
