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
public class PedidoService_AtualizarStatusAsyncTeste
{
    private readonly Mock<IPedidoCommandRepository> mockPedidoCommand;
    private readonly Mock<IPedidoQueryRepository> mockPedidoQuery;
    private readonly Mock<IStatusPedidoHistoricoCommandRepository> mockStatusHistoricoPedidoCommand;
    private readonly Mock<IEventPublisher> mockEventPublisher;
    private readonly Services.PedidoService pedidoService;

    public PedidoService_AtualizarStatusAsyncTeste()
    {
        mockPedidoCommand = new Mock<IPedidoCommandRepository>();
        mockPedidoQuery = new Mock<IPedidoQueryRepository>();
        mockStatusHistoricoPedidoCommand = new Mock<IStatusPedidoHistoricoCommandRepository>();
        mockEventPublisher = new Mock<IEventPublisher>();

        pedidoService = new Services.PedidoService(mockPedidoCommand.Object, mockPedidoQuery.Object, mockStatusHistoricoPedidoCommand.Object, mockEventPublisher.Object);
    }

    [Fact]
    public async Task AtualizarStatus_ComPedidoExistente_DeveAtualizarComSucesso()
    {
        // Arrange
        var pedidoExistente = new Domain.Entities.Pedido(
            new ClientePedido(Guid.NewGuid(), "Maria", "maria@email.com"),
            FormaEntrega.Delivery
        );

        var dto = new PedidoUpdateStatusDto
        {
            IdPedido = pedidoExistente.Id,
            NovoStatus = StatusPedido.EmPreparo,
            IdFuncionario = Guid.NewGuid(),
            Observacao = "Iniciando preparo"
        };

        mockPedidoCommand
            .Setup(repo => repo.ObterPorIdAsync(dto.IdPedido))
            .ReturnsAsync(pedidoExistente);

        mockPedidoCommand
            .Setup(repo => repo.SalvarAlteracoesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await pedidoService.AtualizarStatusAsync(dto);

        // Assert
        mockPedidoCommand.Verify(repo => repo.ObterPorIdAsync(dto.IdPedido), Times.Once);
        mockPedidoCommand.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task AtualizarStatus_ComPedidoInexistente_DeveLancarExcecao()
    {
        // Arrange
        var dto = new PedidoUpdateStatusDto
        {
            IdPedido = Guid.NewGuid(),
            NovoStatus = StatusPedido.Pronto,
            IdFuncionario = Guid.NewGuid(),
            Observacao = "Pedido finalizado"
        };

        mockPedidoCommand
            .Setup(repo => repo.ObterPorIdAsync(dto.IdPedido))
            .ReturnsAsync((Domain.Entities.Pedido?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => pedidoService.AtualizarStatusAsync(dto));

        mockPedidoCommand.Verify(repo => repo.ObterPorIdAsync(dto.IdPedido), Times.Once);
        mockPedidoCommand.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Never);
    }
}
