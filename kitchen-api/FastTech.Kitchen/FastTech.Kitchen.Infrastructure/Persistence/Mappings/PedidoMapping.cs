using FastTech.Kitchen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastTech.Kitchen.Infrastructure.Persistence.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(p => p.Itens)
                .WithOne()
                .HasForeignKey("PedidoId");

            builder.ToTable("Pedidos");
        }
    }
}
