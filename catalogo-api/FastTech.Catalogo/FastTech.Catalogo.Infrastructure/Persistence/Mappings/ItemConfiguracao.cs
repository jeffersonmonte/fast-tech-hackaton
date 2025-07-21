using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FastTech.Catalogo.Infrastructure.Persistence.Mappings
{
    public class ItemConfiguracao : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Nome)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(i => i.Descricao)
                .HasMaxLength(500);

            builder.Property(i => i.DataCriacao)
                .IsRequired();

            builder.Property(i => i.DataEdicao);

            builder.Property(i => i.DataExclusao);

            builder.OwnsOne(i => i.Preco, preco =>
            {
                preco.Property(p => p.Valor)
                    .HasColumnName("Preco")
                    .IsRequired();
            });

            builder.HasOne(i => i.TipoRefeicao)
                .WithMany(t => t.Itens)
                .HasForeignKey(i => i.TipoRefeicaoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(i => i.Cardapios)
                .WithMany(c => c.Itens)
                .UsingEntity<Dictionary<string, object>>(
                    "CardapioItem",
                    j => j.HasOne<Cardapio>().WithMany().HasForeignKey("CardapioId"),
                    j => j.HasOne<Item>().WithMany().HasForeignKey("ItemId"));
        }
    }
}
