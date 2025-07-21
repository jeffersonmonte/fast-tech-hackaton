using System;
using System.Collections.Generic;
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
public class PedidoService_CriarPedidoAsyncTeste
{
    private readonly Mock<IPedidoRepository> mockPedidoRepository;
    private readonly Services.PedidoService pedidoService;

    public PedidoService_CriarPedidoAsyncTeste()
    {
        mockPedidoRepository = new Mock<IPedidoRepository>();
        pedidoService = new Services.PedidoService(mockPedidoRepository.Object);
    }

    [Fact]
    public async Task CriarPedido_ComDadosValidos_DeveCriarComSucesso()
    {
        // Arrange
        var pedidoInput = new PedidoInputDto
        {
            IdCliente = Guid.NewGuid(),
            Cliente = new ClientePedidoDto
            {
                IdCliente = Guid.NewGuid(),
                Nome = "João",
                Email = "joao@email.com"
            },
            FormaEntrega = FormaEntrega.Balcao,
            Itens =
            [
                new()
                {
                    IdItemCardapio = Guid.NewGuid(),
                    Nome = "Hambúrguer",
                    PrecoUnitario = 20,
                    Quantidade = 2
                }
            ]
        };

        mockPedidoRepository
            .Setup(repo => repo.AdicionarAsync(It.IsAny<Domain.Entities.Pedido>()))
            .Returns(Task.CompletedTask);

        mockPedidoRepository
            .Setup(repo => repo.SalvarAlteracoesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var id = await pedidoService.CriarPedidoAsync(pedidoInput);

        // Assert
        mockPedidoRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
        mockPedidoRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
        Assert.NotEqual(Guid.Empty, id);
    }
}