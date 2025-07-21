using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Interfaces;
using FastTech.Pedido.Infraestructure.Persistance.Command;
using FastTech.Pedido.Infraestructure.Persistance.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Infraestructure.Repositories
{
    public class ItemPedidoRepository : RepositoryBase<ItemPedido>, IItemPedidoRepository
    {
        public ItemPedidoRepository(PedidoCommandDbContext commandContext, PedidoQueryDbContext queryContext) : base(commandContext, queryContext)
        {
        }
    }
}
