using FastTech.Pedido.Application.Dtos;
using FastTech.Pedido.Application.Interfaces;
using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using FastTech.Pedido.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoCommandRepository _pedidoCommand;
        private readonly IPedidoQueryRepository _pedidoQuery;
        private readonly IStatusPedidoHistoricoCommandRepository _statusPedidoHistoricoCommand;
        private readonly IEventPublisher _eventPublisher;

        public PedidoService(
            IPedidoCommandRepository pedidoCommand
            , IPedidoQueryRepository pedidoQuery
            , IStatusPedidoHistoricoCommandRepository statusPedidoHistoricoCommand
            , IEventPublisher eventPublisher
            )
        {
            _pedidoCommand = pedidoCommand;
            _pedidoQuery = pedidoQuery;
            _statusPedidoHistoricoCommand = statusPedidoHistoricoCommand;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> CriarPedidoAsync(PedidoInputDto dto)
        {
            var cliente = new ClientePedido(dto.Cliente.IdCliente, dto.Cliente.Nome, dto.Cliente.Email);
            var pedido = new Domain.Entities.Pedido(cliente, dto.FormaEntrega);

            foreach (var item in dto.Itens)
                pedido.AdicionarItem(item.IdItemCardapio, item.Nome, item.PrecoUnitario, item.Quantidade);

            await _pedidoCommand.AdicionarAsync(pedido);
            await _pedidoCommand.SalvarAlteracoesAsync();

            await _eventPublisher.PublishAsync("pedido.pedido", "pedido.created", new PedidoEventDtoCreated
            {
                Id = pedido.Id,
                IdCliente = cliente.IdCliente,
                NomeCliente = cliente.Nome,
                EmailCliente = cliente.Email,
                FormaEntrega = pedido.FormaEntrega,
                Itens = pedido.Itens.Select(i => new ItemPedidoInputDto
                {
                    IdItemCardapio = i.Id,
                    Nome = i.Nome,
                    PrecoUnitario = i.PrecoUnitario,
                    Quantidade = i.Quantidade
                }).ToList()
            });

            return pedido.Id;
        }

        public async Task CancelarPedidoAsync(PedidoCancelamentoDto dto)
        {
            var pedido = await _pedidoCommand.ObterPorIdAsync(dto.IdPedido)
                          ?? throw new InvalidOperationException("Pedido não encontrado.");

            pedido.Cancelar(dto.Justificativa);

            var ultimoHistorico = pedido.DesempilharHistorico();
            if(ultimoHistorico is not null)
                await _statusPedidoHistoricoCommand.AdicionarAsync(ultimoHistorico);

            await _pedidoCommand.SalvarAlteracoesAsync();
        }

        public async Task AtualizarStatusAsync(PedidoUpdateStatusDto dto)
        {
            var pedido = await _pedidoCommand.ObterPorIdAsync(dto.IdPedido)
                          ?? throw new InvalidOperationException("Pedido não encontrado.");

            pedido.AtualizarStatus(dto.NovoStatus, dto.IdFuncionario, dto.Observacao);

            var ultimoHistorico = pedido.DesempilharHistorico();
            if (ultimoHistorico is not null)
                await _statusPedidoHistoricoCommand.AdicionarAsync(ultimoHistorico);

            await _pedidoCommand.SalvarAlteracoesAsync();
        }

        public async Task<PedidoOutputDto?> ObterPedidoCompletoAsync(Guid idPedido)
        {
            var pedido = await _pedidoQuery.ObterPorIdAsync(idPedido);
            return pedido == null ? null : MapearParaOutputDto(pedido);
        }

        public async Task<IEnumerable<PedidoOutputDto>> ListarPedidosClienteAsync(Guid idCliente)
        {
            var pedidos = await _pedidoQuery.ListarAsync(p => p.Cliente.IdCliente == idCliente && p.DataHoraCancelamento == null);
            return pedidos.Select(MapearParaOutputDto);
        }

        private PedidoOutputDto MapearParaOutputDto(Domain.Entities.Pedido pedido)
        {
            return new PedidoOutputDto
            {
                Id = pedido.Id,
                CodigoPedido = pedido.CodigoPedido,
                NomeCliente = pedido.Cliente.Nome,
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
