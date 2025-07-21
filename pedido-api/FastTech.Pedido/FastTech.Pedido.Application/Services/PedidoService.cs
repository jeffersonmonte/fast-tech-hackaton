using FastTech.Pedido.Application.Dtos;
using FastTech.Pedido.Application.Interfaces;
using FastTech.Pedido.Domain.Interfaces;
using FastTech.Pedido.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<Guid> CriarPedidoAsync(PedidoInputDto dto)
        {
            var cliente = new ClientePedido(dto.Cliente.IdCliente, dto.Cliente.Nome, dto.Cliente.Email);
            var pedido = new Domain.Entities.Pedido(dto.IdCliente, cliente, dto.FormaEntrega);

            foreach (var item in dto.Itens)
                pedido.AdicionarItem(item.IdItemCardapio, item.Nome, item.PrecoUnitario, item.Quantidade);

            await _pedidoRepository.AdicionarAsync(pedido);
            await _pedidoRepository.SalvarAlteracoesAsync();

            return pedido.Id;
        }

        public async Task CancelarPedidoAsync(PedidoCancelamentoDto dto)
        {
            var pedido = await _pedidoRepository.ObterPorId(dto.IdPedido)
                          ?? throw new InvalidOperationException("Pedido não encontrado.");

            pedido.Cancelar(dto.Justificativa);

            _pedidoRepository.Atualizar(pedido);
            await _pedidoRepository.SalvarAlteracoesAsync();
        }

        public async Task AtualizarStatusAsync(PedidoUpdateStatusDto dto)
        {
            var pedido = await _pedidoRepository.ObterPorId(dto.IdPedido)
                          ?? throw new InvalidOperationException("Pedido não encontrado.");

            pedido.AtualizarStatus(dto.NovoStatus, dto.IdFuncionario, dto.Observacao);

            _pedidoRepository.Atualizar(pedido);
            await _pedidoRepository.SalvarAlteracoesAsync();
        }

        public async Task<PedidoOutputDto?> ObterPedidoCompletoAsync(Guid idPedido)
        {
            var pedido = await _pedidoRepository.ObterPorId(idPedido);
            return pedido == null ? null : MapearParaOutputDto(pedido);
        }

        public async Task<IEnumerable<PedidoOutputDto>> ListarPedidosClienteAsync(Guid idCliente)
        {
            var pedidos = await _pedidoRepository.ListarAsync(p => p.IdCliente == idCliente);
            return pedidos.Select(MapearParaOutputDto);
        }

        private PedidoOutputDto MapearParaOutputDto(Domain.Entities.Pedido pedido)
        {
            return new PedidoOutputDto
            {
                Id = pedido.Id,
                CodigoPedido = pedido.CodigoPedido,
                Status = pedido.Status.ToString(),
                FormaEntrega = pedido.FormaEntrega.ToString(),
                ValorTotal = pedido.ValorTotal,
                DataHoraCriacao = pedido.DataHoraCriacao,
                Itens = pedido.Itens.Select(i => new ItemPedidoOutputDto
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    PrecoUnitario = i.PrecoUnitario,
                    Quantidade = i.Quantidade,
                    Subtotal = i.Subtotal
                }).ToList(),
                Historico = pedido.Historico.Select(h => new StatusPedidoHistoricoOutputDto
                {
                    Status = h.Status.ToString(),
                    DataHora = h.DataHora,
                    IdFuncionarioResponsavel = h.IdFuncionarioResponsavel,
                    Observacao = h.Observacao
                }).ToList()
            };
        }
    }
}
