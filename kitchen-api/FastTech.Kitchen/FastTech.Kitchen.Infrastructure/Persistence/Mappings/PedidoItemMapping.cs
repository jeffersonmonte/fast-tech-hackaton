using FastTech.Kitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastTech.Kitchen.Infrastructure.Persistence.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.HasKey("Id"); // Chave primária EF Core

            builder.Property(pi => pi.NomeProduto)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pi => pi.Quantidade)
                .IsRequired();

            builder.ToTable("PedidoItens");
        }
    }
}
