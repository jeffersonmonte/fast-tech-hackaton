using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

public class CardapioService_ObterTodosAsyncTeste
{
    private readonly Mock<ICardapioRepository> mockRepository;
    private readonly Mock<IItemRepository> mockItemRepository;
    private readonly Services.CardapioService cardapioService;

    public CardapioService_ObterTodosAsyncTeste()
    {
        mockRepository = new Mock<ICardapioRepository>();
        mockItemRepository = new Mock<IItemRepository>();
        cardapioService = new Services.CardapioService(mockRepository.Object, mockItemRepository.Object);
    }

    [Fact]
    public async Task ObterTodosAsync_ComCardapiosExistentes_DeveRetornarListaDeCardapios()
    {
        // Arrange
        var cardapiosEsperados = new List<Cardapio>
        {
            new("Cardapio 1", "Descricao 1", DateTime.UtcNow),
            new("Cardapio 2", "Descricao 2", DateTime.UtcNow)
        };

        mockRepository
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync(cardapiosEsperados);

        // Act
        var resultado = (await cardapioService.ObterTodosAsync()).ToList();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(cardapiosEsperados.Count, resultado.Count);
        Assert.Equal(cardapiosEsperados[0].Nome, resultado[0].Nome);
        Assert.Equal(cardapiosEsperados[1].Nome, resultado[1].Nome);
        mockRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAsync_SemCardapios_DeveRetornarListaVazia()
    {
        // Arrange
        mockRepository
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync([]);

        // Act
        var resultado = await cardapioService.ObterTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
        mockRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAsync_ComCardapiosSemItens_DeveRetornarListaDeCardapiosVazios()
    {
        // Arrange
        var cardapiosEsperados = new List<Cardapio>
        {
            new("Cardapio 1", "Descricao 1", DateTime.UtcNow),
            new("Cardapio 2", "Descricao 2", DateTime.UtcNow)
        };

        mockRepository
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync(cardapiosEsperados);

        // Act
        var resultado = await cardapioService.ObterTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.All(resultado, cardapio => Assert.Empty(cardapio.Itens!));
        mockRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }
}
