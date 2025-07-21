using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastTech.Pedido.Application.Services;
using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Enums;
using FastTech.Pedido.Domain.Interfaces;
using FastTech.Pedido.Domain.ValueObjects;
using Moq;
using Xunit;

namespace FastTech.Pedido.Application.Test.Unitario.PedidoService;

[Trait("Category", "Unit")]
public class PedidoService_ListarPedidosClienteAsyncTeste
{
    private readonly Mock<IPedidoRepository> mockPedidoRepository;
    private readonly Services.PedidoService pedidoService;

    public PedidoService_ListarPedidosClienteAsyncTeste()
    {
        mockPedidoRepository = new Mock<IPedidoRepository>();
        pedidoService = new Services.PedidoService(mockPedidoRepository.Object);
    }

    [Fact]
    public async Task ListarPedidosCliente_ComPedidosExistentes_DeveRetornarLista()
    {
        // Arrange
        var idCliente = Guid.NewGuid();

        var pedido1 = new Domain.Entities.Pedido(idCliente, new ClientePedido(idCliente, "Ana", "ana@email.com"), FormaEntrega.Balcao);
        var pedido2 = new Domain.Entities.Pedido(idCliente, new ClientePedido(idCliente, "Ana", "ana@email.com"), FormaEntrega.Delivery);

        mockPedidoRepository
            .Setup(repo => repo.ListarAsync(p => p.IdCliente == idCliente))
            .ReturnsAsync([pedido1, pedido2]);

        // Act
        var resultado = await pedidoService.ListarPedidosClienteAsync(idCliente);

        // Assert
        mockPedidoRepository.Verify(repo => repo.ListarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Pedido, bool>>>()), Times.Once);
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

        mockPedidoRepository
            .Setup(repo => repo.ListarAsync(p => p.IdCliente == idCliente))
            .ReturnsAsync([]);

        // Act
        var resultado = await pedidoService.ListarPedidosClienteAsync(idCliente);

        // Assert
        mockPedidoRepository.Verify(repo => repo.ListarAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Pedido, bool>>>()), Times.Once);
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }
}