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
    public class ItemPedidoCommandRepository : CommandRepositoryBase<ItemPedido>, IItemPedidoCommandRepository
    {
        public ItemPedidoCommandRepository(PedidoCommandDbContext context) : base(context) { }
    }
}
