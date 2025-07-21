using System;
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
public class PedidoService_ObterPedidoCompletoAsyncTeste
{
    private readonly Mock<IPedidoRepository> mockPedidoRepository;
    private readonly Services.PedidoService pedidoService;

    public PedidoService_ObterPedidoCompletoAsyncTeste()
    {
        mockPedidoRepository = new Mock<IPedidoRepository>();
        pedidoService = new Services.PedidoService(mockPedidoRepository.Object);
    }

    [Fact]
    public async Task ObterPedidoCompleto_ComPedidoExistente_DeveRetornarDto()
    {
        // Arrange
        var pedido = new Domain.Entities.Pedido(
            Guid.NewGuid(),
            new ClientePedido(Guid.NewGuid(), "Carlos", "carlos@email.com"),
            FormaEntrega.DriveThru
        );

        mockPedidoRepository
            .Setup(repo => repo.ObterPorId(pedido.Id))
            .ReturnsAsync(pedido);

        // Act
        var resultado = await pedidoService.ObterPedidoCompletoAsync(pedido.Id);

        // Assert
        mockPedidoRepository.Verify(repo => repo.ObterPorId(pedido.Id), Times.Once);
        Assert.NotNull(resultado);
        Assert.Equal(pedido.Id, resultado!.Id);
        Assert.Equal(pedido.CodigoPedido, resultado.CodigoPedido);
    }

    [Fact]
    public async Task ObterPedidoCompleto_ComPedidoInexistente_DeveRetornarNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        mockPedidoRepository
            .Setup(repo => repo.ObterPorId(id))
            .ReturnsAsync((Domain.Entities.Pedido?)null);

        // Act
        var resultado = await pedidoService.ObterPedidoCompletoAsync(id);

        // Assert
        mockPedidoRepository.Verify(repo => repo.ObterPorId(id), Times.Once);
        Assert.Null(resultado);
    }
}
