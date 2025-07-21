using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using FastTech.Pedido.Infrastructure.Persistance.Command;
using FastTech.Pedido.Infrastructure.Persistance.Query;
using FastTech.Pedido.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastTech.Pedido.Infrastructure.Repositories.Query
{
    public class ItemPedidoQueryRepository : QueryRepositoryBase<ItemPedido>, IItemPedidoQueryRepository
    {
        public ItemPedidoQueryRepository(PedidoQueryDbContext context) : base(context) { }
    }
}
