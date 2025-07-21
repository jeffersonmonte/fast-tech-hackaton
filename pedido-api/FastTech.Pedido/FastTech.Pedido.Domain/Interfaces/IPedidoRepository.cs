using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Interfaces
{
    public interface IPedidoRepository : IRepositoryBase<Entities.Pedido>
    {
        Task<Entities.Pedido?> ObterPorId(Guid id);
    }
}
