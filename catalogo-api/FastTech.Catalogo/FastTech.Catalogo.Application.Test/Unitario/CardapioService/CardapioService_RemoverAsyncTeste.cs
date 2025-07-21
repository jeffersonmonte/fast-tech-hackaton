using System;
using System.Threading.Tasks;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

public class CardapioService_RemoverTeste
{
    private readonly Mock<ICardapioRepository> mockRepository;
    private readonly Mock<IItemRepository> mockItemRepository;
    private readonly Services.CardapioService cardapioService;

    public CardapioService_RemoverTeste()
    {
        mockRepository = new Mock<ICardapioRepository>();
        mockItemRepository = new Mock<IItemRepository>();
        cardapioService = new Services.CardapioService(mockRepository.Object, mockItemRepository.Object);
    }

    [Fact]
    public async Task Remover_ComIdValido_DeveRemoverComSucesso()
    {
        // Arrange
        var cardapioASeDeletar = new Cardapio("Cardapio Teste", "Descricao Teste", DateTime.UtcNow);

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(cardapioASeDeletar.Id))
            .ReturnsAsync(cardapioASeDeletar);

        mockRepository
            .Setup(repo => repo.Atualizar(cardapioASeDeletar));

        // Act
        await cardapioService.RemoverAsync(cardapioASeDeletar.Id);

        // Assert
        mockRepository.Verify(repo => repo.Atualizar(cardapioASeDeletar), Times.Once);
    }

    [Fact]
    public async Task Remover_ComIdInvalido_DeveLancarExcecao()
    {
        // Arrange
        var cardapioASeDeletar = new Cardapio("Cardapio Teste", "Descricao Teste", DateTime.UtcNow);

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(cardapioASeDeletar.Id))
            .ReturnsAsync((Cardapio)null!);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => cardapioService.RemoverAsync(cardapioASeDeletar.Id));
        mockRepository.Verify(repo => repo.Remover(It.IsAny<Cardapio>()), Times.Never);
    }
}
