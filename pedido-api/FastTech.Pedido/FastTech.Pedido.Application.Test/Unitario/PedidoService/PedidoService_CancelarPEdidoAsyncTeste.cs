using System;
using System.Threading.Tasks;
using FastTech.Pedido.Application.Dtos;
using FastTech.Pedido.Application.Services;
using FastTech.Pedido.Domain.Entities;
using FastTech.Pedido.Domain.Enums;
using FastTech.Pedido.Domain.Interfaces;
using FastTech.Pedido.Domain.ValueObjects;
using Moq;
using Xunit;

namespace FastTech.Pedido.Application.Test.Unitario.PedidoService;

[Trait("Category", "Unit")]
public class PedidoService_CancelarPedidoAsyncTeste
{
    private readonly Mock<IPedidoRepository> mockPedidoRepository;
    private readonly Services.PedidoService pedidoService;

    public PedidoService_CancelarPedidoAsyncTeste()
    {
        mockPedidoRepository = new Mock<IPedidoRepository>();
        pedidoService = new Services.PedidoService(mockPedidoRepository.Object);
    }

    [Fact]
    public async Task CancelarPedido_ComPedidoExistente_DeveCancelarComSucesso()
    {
        // Arrange
        var pedidoExistente = new Domain.Entities.Pedido(
            Guid.NewGuid(),
            new ClientePedido(Guid.NewGuid(), "João", "joao@email.com"),
            FormaEntrega.Balcao
        );

        var dto = new PedidoCancelamentoDto
        {
            IdPedido = pedidoExistente.Id,
            Justificativa = "Cliente desistiu"
        };

        mockPedidoRepository
            .Setup(repo => repo.ObterPorId(dto.IdPedido))
            .ReturnsAsync(pedidoExistente);

        mockPedidoRepository
            .Setup(repo => repo.SalvarAlteracoesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await pedidoService.CancelarPedidoAsync(dto);

        // Assert
        mockPedidoRepository.Verify(repo => repo.ObterPorId(dto.IdPedido), Times.Once);
        mockPedidoRepository.Verify(repo => repo.Atualizar(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
        mockPedidoRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
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

        mockPedidoRepository
            .Setup(repo => repo.ObterPorId(dto.IdPedido))
            .ReturnsAsync((Domain.Entities.Pedido?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => pedidoService.CancelarPedidoAsync(dto));

        mockPedidoRepository.Verify(repo => repo.ObterPorId(dto.IdPedido), Times.Once);
        mockPedidoRepository.Verify(repo => repo.Atualizar(It.IsAny<Domain.Entities.Pedido>()), Times.Never);
        mockPedidoRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Never);
    }
}