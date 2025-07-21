using FastTech.Pedido.Application.Dtos;
using FastTech.Pedido.Application.Interfaces;
using FastTech.Pedido.Application.Services;
using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Enums;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using FastTech.Pedido.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Pedido.Application.Test.Unitario.PedidoService;

[Trait("Category", "Unit")]
public class PedidoService_CancelarPedidoAsyncTeste
{
    private readonly Mock<IPedidoCommandRepository> mockPedidoCommand;
    private readonly Mock<IPedidoQueryRepository> mockPedidoQuery;
    private readonly Mock<IStatusPedidoHistoricoCommandRepository> mockStatusHistoricoPedidoCommand;
    private readonly Mock<IEventPublisher> mockEventPublisher;

    private readonly Services.PedidoService pedidoService;

    public PedidoService_CancelarPedidoAsyncTeste()
    {
        mockPedidoCommand = new Mock<IPedidoCommandRepository>();
        mockPedidoQuery = new Mock<IPedidoQueryRepository>();
        mockStatusHistoricoPedidoCommand = new Mock<IStatusPedidoHistoricoCommandRepository>();
        mockEventPublisher = new Mock<IEventPublisher>();


        pedidoService = new Services.PedidoService(mockPedidoCommand.Object, mockPedidoQuery.Object, mockStatusHistoricoPedidoCommand.Object, mockEventPublisher.Object);
    }

    [Fact]
    public async Task CancelarPedido_ComPedidoExistente_DeveCancelarComSucesso()
    {
        // Arrange
        var pedidoExistente = new Domain.Entities.Pedido(
            new ClientePedido(Guid.NewGuid(), "João", "joao@email.com"),
            FormaEntrega.Balcao
        );

        var dto = new PedidoCancelamentoDto
        {
            IdPedido = pedidoExistente.Id,
            Justificativa = "Cliente desistiu"
        };

        mockPedidoCommand
            .Setup(repo => repo.ObterPorIdAsync(dto.IdPedido))
            .ReturnsAsync(pedidoExistente);

        mockPedidoCommand
            .Setup(repo => repo.SalvarAlteracoesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await pedidoService.CancelarPedidoAsync(dto);

        // Assert
        mockPedidoCommand.Verify(repo => repo.ObterPorIdAsync(dto.IdPedido), Times.Once);
        mockPedidoCommand.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task CancelarPedido_ComPedidoInexistente_DeveLancarExcecao()
    {
        // Arrange
        var dto = new PedidoCancelamentoDto
        {
            IdPedido = Guid.NewGuid(),
            Justificativa = "Cancelamento inválido"
        };

        mockPedidoCommand
            .Setup(repo => repo.ObterPorIdAsync(dto.IdPedido))
            .ReturnsAsync((Domain.Entities.Pedido?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => pedidoService.CancelarPedidoAsync(dto));

        mockPedidoCommand.Verify(repo => repo.ObterPorIdAsync(dto.IdPedido), Times.Once);
        mockPedidoCommand.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Never);
    }
}
