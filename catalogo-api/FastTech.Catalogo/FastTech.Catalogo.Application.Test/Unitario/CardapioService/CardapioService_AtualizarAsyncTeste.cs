using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using Moq;
using System;
using System.Linq.Expressions;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

public class CardapioService_AtualizarAsyncTeste
{
    private readonly Mock<IItemRepository> mockItemRepository;
    private readonly Mock<ICardapioRepository> mockRepository;
    private readonly Services.CardapioService cardapioService;

    public CardapioService_AtualizarAsyncTeste()
    {
        mockItemRepository = new Mock<IItemRepository>();
        mockRepository = new Mock<ICardapioRepository>();
        cardapioService = new Services.CardapioService(mockRepository.Object, mockItemRepository.Object);
    }

    [Fact]
    public async Task AtualizarCardapio_ComNovoNomeEDescricao_DeveAtualizarComSucesso()
    {
        // Arrange
        var item = new Item("Nome", "Descricao", new("Tipo"), new(20));
        var cardapioExistente = new Cardapio("Cardapio Antigo", "Descricao Antiga", DateTime.UtcNow);
        var atualizarDto = new CardapioUpdateDto
        {
            Id = cardapioExistente.Id,
            Nome = "Cardapio Atualizado",
            Descricao = "Descricao Atualizada",
            ItensIds = [item.Id]
        };

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(cardapioExistente.Id))
            .ReturnsAsync(cardapioExistente);

        mockRepository
            .Setup(repo => repo.Atualizar(It.IsAny<Cardapio>()));

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([item]);

        // Act
        await cardapioService.AtualizarAsync(atualizarDto);

        // Assert
        mockRepository.Verify(repo => repo.Atualizar(It.IsAny<Cardapio>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarCardapio_ComIdInexistente_DeveLancarExcecao()
    {
        // Arrange
        var atualizarDto = new CardapioUpdateDto
        {
            Id = Guid.NewGuid(),
            Nome = "Cardapio Atualizado",
            Descricao = "Descricao Atualizada",
            ItensIds = [Guid.NewGuid()]
        };

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Cardapio)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => cardapioService.AtualizarAsync(atualizarDto));
        mockRepository.Verify(repo => repo.Atualizar(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarCardapio_ComItensNaoExistentes_DeveLancarExcecao()
    {
        // Arrange
        var cardapioExistente = new Cardapio("Cardapio Antigo", "Descricao Antiga", DateTime.UtcNow);
        var atualizarDto = new CardapioUpdateDto
        {
            Id = cardapioExistente.Id,
            Nome = "Cardapio Atualizado",
            Descricao = "Descricao Atualizada",
            ItensIds = [Guid.NewGuid()]
        };

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(cardapioExistente.Id))
            .ReturnsAsync(cardapioExistente);

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([]);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => cardapioService.AtualizarAsync(atualizarDto));
        mockRepository.Verify(repo => repo.Atualizar(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AtualizarCardapio_ComNomeDuplicado_DeveLancarExcecao()
    {
        // Arrange
        var item = new Item("Nome", "Descricao", new("Tipo"), new(20));
        var cardapioExistente = new Cardapio("Cardapio Antigo", "Descricao Antiga", DateTime.UtcNow);
        var atualizarDto = new CardapioUpdateDto
        {
            Id = cardapioExistente.Id,
            Nome = "NomeDuplicado",
            Descricao = "Descricao Atualizada",
            ItensIds = [item.Id]
        };

        mockRepository
            .Setup(repo => repo.ObterPorIdAsync(cardapioExistente.Id))
            .ReturnsAsync(cardapioExistente);

        mockRepository
            .Setup(repo => repo.ObterPorNomeAsync(atualizarDto.Nome))
            .ReturnsAsync(new Cardapio("NomeDuplicado", "Descricao Atualizada", DateTime.Now));

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([item]);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => cardapioService.AtualizarAsync(atualizarDto));
        mockRepository.Verify(repo => repo.Atualizar(It.IsAny<Cardapio>()), Times.Never);
    }
}
