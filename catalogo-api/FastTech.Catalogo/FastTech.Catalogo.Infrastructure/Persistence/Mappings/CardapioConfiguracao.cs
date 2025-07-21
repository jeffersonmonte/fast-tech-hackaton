using FastTech.Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FastTech.Catalogo.Infrastructure.Persistence.Mappings
{
    public class CardapioConfiguracao : IEntityTypeConfiguration<Cardapio>
    {
        public void Configure(EntityTypeBuilder<Cardapio> builder)
        {
            builder.ToTable("Cardapio");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.Descricao)
                .HasMaxLength(500);

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            builder.Property(c => c.DataEdicao);

            builder.Property(c => c.DataExclusao);

            builder.HasMany(c => c.Itens)
                .WithMany(i => i.Cardapios)
                .UsingEntity<Dictionary<string, object>>(
                    "CardapioItem",
                    j => j.HasOne<Item>().WithMany().HasForeignKey("ItemId"),
                    j => j.HasOne<Cardapio>().WithMany().HasForeignKey("CardapioId"));
        }
    }
}
