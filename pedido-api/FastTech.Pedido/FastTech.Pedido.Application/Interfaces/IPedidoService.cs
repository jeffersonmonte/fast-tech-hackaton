using FastTech.Pedido.Application.Dtos;
using FastTech.Pedido.Domain.Enums;
using FastTech.Pedido.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<Guid> CriarPedidoAsync(PedidoInputDto dto);
        Task CancelarPedidoAsync(PedidoCancelamentoDto dto);
        Task AtualizarStatusAsync(PedidoUpdateStatusDto dto);
        Task<PedidoOutputDto?> ObterPedidoCompletoAsync(Guid idPedido);
        Task<IEnumerable<PedidoOutputDto>> ListarPedidosClienteAsync(Guid idCliente);
    }
}
