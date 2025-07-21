using FastTech.Kitchen.Domain.Entities;
using FastTech.Kitchen.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastTech.Kitchen.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly List<Pedido> _pedidos = new List<Pedido>();

        public Task Adicionar(Pedido pedido)
        {
            _pedidos.Add(pedido);
            return Task.CompletedTask;
        }

        public Task<Pedido> ObterPorId(Guid id)
        {
            return Task.FromResult(_pedidos.FirstOrDefault(p => p.Id == id));
        }

        public Task<IEnumerable<Pedido>> ObterTodos()
        {
            return Task.FromResult(_pedidos.AsEnumerable());
        }

        public Task Atualizar(Pedido pedido)
        {
            var index = _pedidos.FindIndex(p => p.Id == pedido.Id);
            if (index != -1)
            {
                _pedidos[index] = pedido;
            }
            return Task.CompletedTask;
        }
    }
}
