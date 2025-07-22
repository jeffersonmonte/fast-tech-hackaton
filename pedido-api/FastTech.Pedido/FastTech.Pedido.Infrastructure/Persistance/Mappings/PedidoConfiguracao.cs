using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FastTech.Pedido.Infraestructure.Persistance.Mappings
{
    public class PedidoConfiguracao : IEntityTypeConfiguration<Domain.Entities.Pedido>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Pedido> builder)
        {
            builder.ToTable("Pedido");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.CodigoPedido)
                .IsRequired()
                .HasMaxLength(20);

            builder.OwnsOne(p => p.Cliente, cliente =>
            {
                cliente.Property(c => c.IdCliente)
                    .HasColumnName("ClienteId")
                    .IsRequired();

                cliente.Property(c => c.Nome)
                    .HasColumnName("ClienteNome")
                    .IsRequired()
                    .HasMaxLength(100);

                cliente.Property(c => c.Email)
                    .HasColumnName("ClienteEmail")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.Property(p => p.DataHoraCriacao)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.FormaEntrega)
                .IsRequired();

            builder.Property(p => p.JustificativaCancelamento)
                .HasMaxLength(500);

            builder.Property(p => p.DataHoraCancelamento);

            builder
                .HasMany(p => p.Itens)
                .WithOne()
                .HasForeignKey("PedidoId");

            builder
                .HasMany(p => p.Historico)
                .WithOne()
                .HasForeignKey("PedidoId");
        }
    }
}
