using FastTech.Pedido.Application.Dtos;
using FastTech.Pedido.Application.Interfaces;
using FastTech.Pedido.Application.Services;
using FastTech.Pedido.Domain.Enums;
using FastTech.Pedido.Domain.Interfaces.Command;
using FastTech.Pedido.Domain.Interfaces.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Pedido.Application.Test.Unitario.PedidoService;

[Trait("Category", "Unit")]
public class PedidoService_CriarPedidoAsyncTeste
{
    private readonly Mock<IPedidoCommandRepository> mockPedidoCommand;
    private readonly Mock<IPedidoQueryRepository> mockPedidoQuery;
    private readonly Mock<IStatusPedidoHistoricoCommandRepository> mockStatusHistoricoPedidoCommand;
    private readonly Mock<IEventPublisher> mockEventPublisher;

    private readonly Services.PedidoService pedidoService;

    public PedidoService_CriarPedidoAsyncTeste()
    {
        mockPedidoCommand = new Mock<IPedidoCommandRepository>();
        mockPedidoQuery = new Mock<IPedidoQueryRepository>();
        mockStatusHistoricoPedidoCommand = new Mock<IStatusPedidoHistoricoCommandRepository>();
        mockEventPublisher = new Mock<IEventPublisher>();


        pedidoService = new Services.PedidoService(mockPedidoCommand.Object, mockPedidoQuery.Object, mockStatusHistoricoPedidoCommand.Object, mockEventPublisher.Object);
    }

    [Fact]
    public async Task CriarPedido_ComDadosValidos_DeveCriarComSucesso()
    {
        // Arrange
        var pedidoInput = new PedidoInputDto
        {
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

        mockPedidoCommand
            .Setup(repo => repo.AdicionarAsync(It.IsAny<Domain.Entities.Pedido>()))
            .Returns(Task.CompletedTask);

        mockPedidoCommand
            .Setup(repo => repo.SalvarAlteracoesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var id = await pedidoService.CriarPedidoAsync(pedidoInput);

        // Assert
        mockPedidoCommand.Verify(repo => repo.AdicionarAsync(It.IsAny<Domain.Entities.Pedido>()), Times.Once);
        mockPedidoCommand.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
        Assert.NotEqual(Guid.Empty, id);
    }
}
