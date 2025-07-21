using FastTech.Pedido.Application.Interfaces;
using FastTech.Pedido.Application.Services;
using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Enums;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using FastTech.Pedido.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Pedido.Application.Test.Unitario.PedidoService;

[Trait("Category", "Unit")]
public class PedidoService_ListarPedidosClienteAsyncTeste
{
    private readonly Mock<IPedidoQueryRepository> mockPedidoQuery;
    private readonly Mock<IPedidoCommandRepository> mockPedidoCommand;
    private readonly Mock<IStatusPedidoHistoricoCommandRepository> mockStatusHistoricoPedidoCommand;
    private readonly Services.PedidoService pedidoService;
    private readonly Mock<IEventPublisher> mockEventPublisher;


    public PedidoService_ListarPedidosClienteAsyncTeste()
    {
        mockPedidoQuery = new Mock<IPedidoQueryRepository>();
        mockPedidoCommand = new Mock<IPedidoCommandRepository>();
        mockStatusHistoricoPedidoCommand = new Mock<IStatusPedidoHistoricoCommandRepository>();
        mockEventPublisher = new Mock<IEventPublisher>();

        pedidoService = new Services.PedidoService(mockPedidoCommand.Object, mockPedidoQuery.Object, mockStatusHistoricoPedidoCommand.Object, mockEventPublisher.Object);
    }

    [Fact]
    public async Task ListarPedidosCliente_ComPedidosExistentes_DeveRetornarLista()
    {
        // Arrange
        var idCliente = Guid.NewGuid();

        var pedido1 = new Domain.Entities.Pedido(new ClientePedido(idCliente, "Ana", "ana@email.com"), FormaEntrega.Balcao);
        var pedido2 = new Domain.Entities.Pedido(new ClientePedido(idCliente, "Ana", "ana@email.com"), FormaEntrega.Delivery);

        mockPedidoQuery
            .Setup(repo => repo.ListarAsync(p => p.Cliente.IdCliente == idCliente && p.DataHoraCancelamento == null))
            .ReturnsAsync([pedido1, pedido2]);

        // Act
        var resultado = await pedidoService.ListarPedidosClienteAsync(idCliente);

        // Assert
        mockPedidoQuery.Verify(repo => repo.ListarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Pedido, bool>>>()), Times.Once);
        Assert.NotNull(resultado);
        Assert.Collection(resultado,
            p => Assert.Equal(pedido1.Id, p.Id),
            p => Assert.Equal(pedido2.Id, p.Id));
    }

    [Fact]
    public async Task ListarPedidosCliente_SemPedidos_DeveRetornarListaVazia()
    {
        // Arrange
        var idCliente = Guid.NewGuid();

        mockPedidoQuery
            .Setup(repo => repo.ListarAsync(p => p.Cliente.IdCliente == idCliente))
            .ReturnsAsync([]);

        // Act
        var resultado = await pedidoService.ListarPedidosClienteAsync(idCliente);

        // Assert
        mockPedidoQuery.Verify(repo => repo.ListarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Pedido, bool>>>()), Times.Once);
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }
}
