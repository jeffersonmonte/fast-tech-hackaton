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
public class PedidoService_ObterPedidoCompletoAsyncTeste
{
    private readonly Mock<IPedidoCommandRepository> mockPedidoCommand;
    private readonly Mock<IPedidoQueryRepository> mockPedidoQuery;
    private readonly Mock<IStatusPedidoHistoricoCommandRepository> mockStatusHistoricoPedidoCommand;
    private readonly Services.PedidoService pedidoService;
    private readonly Mock<IEventPublisher> mockEventPublisher;


    public PedidoService_ObterPedidoCompletoAsyncTeste()
    {
        mockPedidoCommand = new Mock<IPedidoCommandRepository>();
        mockPedidoQuery = new Mock<IPedidoQueryRepository>();
        mockStatusHistoricoPedidoCommand = new Mock<IStatusPedidoHistoricoCommandRepository>();
        mockEventPublisher = new Mock<IEventPublisher>();

        pedidoService = new Services.PedidoService(mockPedidoCommand.Object, mockPedidoQuery.Object, mockStatusHistoricoPedidoCommand.Object, mockEventPublisher.Object);
    }

    [Fact]
    public async Task ObterPedidoCompleto_ComPedidoExistente_DeveRetornarDto()
    {
        // Arrange
        var pedido = new Domain.Entities.Pedido(
            new ClientePedido(Guid.NewGuid(), "Carlos", "carlos@email.com"),
            FormaEntrega.DriveThru
        );

        mockPedidoQuery
            .Setup(repo => repo.ObterPorIdAsync(pedido.Id))
            .ReturnsAsync(pedido);

        // Act
        var resultado = await pedidoService.ObterPedidoCompletoAsync(pedido.Id);

        // Assert
        mockPedidoQuery.Verify(repo => repo.ObterPorIdAsync(pedido.Id), Times.Once);
        Assert.NotNull(resultado);
        Assert.Equal(pedido.Id, resultado!.Id);
        Assert.Equal(pedido.CodigoPedido, resultado.CodigoPedido);
    }

    [Fact]
    public async Task ObterPedidoCompleto_ComPedidoInexistente_DeveRetornarNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        mockPedidoQuery
            .Setup(repo => repo.ObterPorIdAsync(id))
            .ReturnsAsync((Domain.Entities.Pedido?)null);

        // Act
        var resultado = await pedidoService.ObterPedidoCompletoAsync(id);

        // Assert
        mockPedidoQuery.Verify(repo => repo.ObterPorIdAsync(id), Times.Once);
        Assert.Null(resultado);
    }
}
