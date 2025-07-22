using FastTech.Pedido.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infraestructure.Persistance.Mappings
{
    public class ItemPedidoConfiguracao : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.ToTable("ItemPedido");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.IdItemCardapio)
                .IsRequired();

            builder.Property(i => i.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.PrecoUnitario)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(i => i.Quantidade)
                .IsRequired();
        }
    }
}
