using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using Moq;
using System;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

public class CardapioService_ObterPorIdAsyncTeste
{
    private readonly Mock<ICardapioRepository> mockRepository;
    private readonly Mock<IItemRepository> mockItemRepository;
    private readonly Services.CardapioService cardapioService;

    public CardapioService_ObterPorIdAsyncTeste()
    {
        mockRepository = new Mock<ICardapioRepository>();
        mockItemRepository = new Mock<IItemRepository>();
        cardapioService = new Services.CardapioService(mockRepository.Object, mockItemRepository.Object);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdValido_DeveRetornarCardapio()
    {
        // Arrange
        var cardapioEsperado = new Cardapio("Cardapio Teste", "Descricao Teste", DateTime.UtcNow);

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(cardapioEsperado.Id))
            .ReturnsAsync(cardapioEsperado);

        // Act
        var resultado = await cardapioService.ObterPorIdAsync(cardapioEsperado.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(cardapioEsperado.Nome, resultado.Nome);
        Assert.Equal(cardapioEsperado.Descricao, resultado.Descricao);
        mockRepository.Verify(repo => repo.ObterPorIdAsync(cardapioEsperado.Id), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdInvalido_DeveRetornarNulo()
    {
        // Arrange
        var cardapioId = Guid.NewGuid();

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(cardapioId))
            .ReturnsAsync((Cardapio)null!);

        // Act
        var resultado = await cardapioService.ObterPorIdAsync(cardapioId);

        // Assert
        Assert.Null(resultado);
        mockRepository.Verify(repo => repo.ObterPorIdAsync(cardapioId), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdVazio_DeveLancarExcecao()
    {
        // Arrange
        var cardapioId = Guid.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => cardapioService.ObterPorIdAsync(cardapioId));
        mockRepository.Verify(repo => repo.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
    }
}
