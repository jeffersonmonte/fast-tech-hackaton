using FastTech.Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FastTech.Catalogo.Infrastructure.Persistence.Mappings
{
    public class TipoRefeicaoConfiguracao : IEntityTypeConfiguration<TipoRefeicao>
    {
        public void Configure(EntityTypeBuilder<TipoRefeicao> builder)
        {
            builder.ToTable("TipoRefeicao");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.DataCriacao)
                .IsRequired();

            builder.Property(t => t.DataEdicao);

            builder.Property(t => t.DataExclusao);

            builder.HasMany(t => t.Itens)
                .WithOne(i => i.TipoRefeicao)
                .HasForeignKey(i => i.TipoRefeicaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
