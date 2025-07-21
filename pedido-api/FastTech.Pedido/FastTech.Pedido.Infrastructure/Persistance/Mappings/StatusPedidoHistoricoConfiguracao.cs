using FastTech.Pedido.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infrastructure.Persistance.Mappings
{
    public class StatusPedidoHistoricoConfiguracao : IEntityTypeConfiguration<StatusPedidoHistorico>
    {
        public void Configure(EntityTypeBuilder<StatusPedidoHistorico> builder)
        {
            builder.ToTable("StatusPedidoHistorico");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Status)
                .IsRequired();

            builder.Property(s => s.DataHora)
                .IsRequired();

            builder.Property(s => s.IdFuncionarioResponsavel);

            builder.Property(s => s.Observacao)
                .HasMaxLength(300);
        }
    }
}
