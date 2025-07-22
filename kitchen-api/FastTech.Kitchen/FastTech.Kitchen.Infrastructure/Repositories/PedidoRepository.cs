using FastTech.Kitchen.Domain.Entities;
using FastTech.Kitchen.Domain.Interfaces;
using FastTech.Kitchen.Infrastructure.Persistence.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastTech.Kitchen.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly KitchenDbContext _context;

        public PedidoRepository(KitchenDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> ObterPorId(Guid id)
        {
            return await _context.Pedidos.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pedido>> ObterTodos()
        {
            return await _context.Pedidos.Include(p => p.Itens).ToListAsync();
        }

        public async Task Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
