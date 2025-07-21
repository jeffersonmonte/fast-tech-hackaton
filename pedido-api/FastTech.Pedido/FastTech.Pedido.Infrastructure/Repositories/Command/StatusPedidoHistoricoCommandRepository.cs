using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using FastTech.Pedido.Infrastructure.Persistance.Query;
using FastTech.Pedido.Infrastructure.Persistance.Command;
using FastTech.Pedido.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infrastructure.Repositories.Command
{
    public class StatusPedidoHistoricoCommandRepository : CommandRepositoryBase<StatusPedidoHistorico>, IStatusPedidoHistoricoCommandRepository
    {
        public StatusPedidoHistoricoCommandRepository(PedidoCommandDbContext context) : base(context) { }
    }
}
