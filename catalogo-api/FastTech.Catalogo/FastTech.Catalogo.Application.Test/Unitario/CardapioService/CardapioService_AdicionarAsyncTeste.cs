using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

[Trait("Category", "Unit")]
public class CardapioService_AdicionarAsyncTeste
{
    // GIVEN
    private readonly Mock<ICardapioRepository> mockRepository;
    private readonly Mock<IItemRepository> mockItemRepository;
    private readonly FastTech.Catalogo.Application.Services.CardapioService cardapioService;

    public CardapioService_AdicionarAsyncTeste()
    {
        mockRepository = new Mock<ICardapioRepository>();
        mockItemRepository = new Mock<IItemRepository>();
        cardapioService = new FastTech.Catalogo.Application.Services.CardapioService(mockRepository.Object, mockItemRepository.Object);
    }

    [Fact]
    public async Task AdicionarCardapio_ComGuidsExistentes_DeveAdicionarComSucesso()
    {
        // Arrange
        var item = new Item("Nome", "Descricao", new("Tipo"), new(20));
        var novoCardapio = new CardapioInputDto
        {
            Nome = "Nome",
            Descricao = "Descrição",
            ItensIds = [item.Id]
        };

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([item]);

        mockRepository
            .Setup(repo => repo.AdicionarAsync(It.IsAny<Cardapio>()))
            .Returns(Task.CompletedTask);

        // Act
        var novoId = await cardapioService.AdicionarAsync(novoCardapio);

        // Assert
        mockRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Cardapio>()), Times.Once);
        Assert.NotEqual(Guid.Empty, novoId);
    }

    [Fact]
    public async Task AdicionarCardapio_ComItensNaoExistentes_DeveLancarExcecao()
    {
        // Arrange
        var novoCardapio = new CardapioInputDto
        {
            Nome = "Nome",
            Descricao = "Descrição",
            ItensIds = [Guid.NewGuid()]
        };

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([]);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => cardapioService.AdicionarAsync(novoCardapio));
        mockRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_SemItens_DeveLancarExcecao()
    {
        // Arrange
        var novoCardapio = new CardapioInputDto
        {
            Nome = "Nome",
            Descricao = "Descrição",
            ItensIds = []
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => cardapioService.AdicionarAsync(novoCardapio));
        mockRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_ComNomeDuplicado_DeveLancarExcecao()
    {
        // Arrange
        var item = new Item("Nome", "Descricao", new("Tipo"), new(20));
        var novoCardapio = new CardapioInputDto
        {
            Nome = "NomeDuplicado",
            Descricao = "Descrição",
            ItensIds = [item.Id]
        };

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([item]);

        mockRepository
            .Setup(repo => repo.ExisteAsync(It.IsAny<Expression<Func<Cardapio, bool>>>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => cardapioService.AdicionarAsync(novoCardapio));
        mockRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_ComNomeVazio_DeveLancarExcecao()
    {
        // Arrange
        var item = new Item("Nome", "Descricao", new("Tipo"), new(20));
        var novoCardapio = new CardapioInputDto
        {
            Nome = string.Empty,
            Descricao = "Descricao",
            ItensIds = [item.Id]
        };

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([item]);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => cardapioService.AdicionarAsync(novoCardapio));
        mockRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_ComItensParcialmenteExistentes_DeveLancarExcecao()
    {
        // Arrange
        var itemExistente = new Item("Nome", "Descricao", new("Tipo"), new(20));
        var itemInexistenteId = Guid.NewGuid();
        var novoCardapio = new CardapioInputDto
        {
            Nome = "Nome",
            Descricao = "Descrição",
            ItensIds = [itemExistente.Id, itemInexistenteId]
        };

        mockItemRepository
            .Setup(repo => repo.ListarAsync(It.IsAny<Expression<Func<Item, bool>>?>()))
            .ReturnsAsync([itemExistente]);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => cardapioService.AdicionarAsync(novoCardapio));
        mockRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }
}
