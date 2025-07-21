using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Query;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService;

[Trait("Category", "Unit")]
public class ItemService_ObterPorIdAsyncTeste
{
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepository;
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockTipoRefeicaoQueryRepository;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly Services.ItemService _itemService;

    public ItemService_ObterPorIdAsyncTeste()
    {
        _mockItemQueryRepository = new Mock<IItemQueryRepository>();
        _mockTipoRefeicaoQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _mockEventPublisher = new Mock<IEventPublisher>();

        _itemService = new Services.ItemService(
            itemCommandRepository: null!,
            _mockItemQueryRepository.Object,
            _mockTipoRefeicaoQueryRepository.Object,
            _mockEventPublisher.Object
        );
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdValido_DeveRetornarItem()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = new Item("Item Teste", "Descrição Teste", Guid.NewGuid(), new Preco(10.0m));
        _mockItemQueryRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync(item);

        // Act
        var result = await _itemService.ObterPorIdAsync(itemId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(item.Nome, result.Nome);
        _mockItemQueryRepository.Verify(repo => repo.ObterPorIdAsync(itemId), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdInvalido_DeveRetornarNulo()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        _mockItemQueryRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync((Item)null!);

        // Act
        var result = await _itemService.ObterPorIdAsync(itemId);

        // Assert
        Assert.Null(result);
        _mockItemQueryRepository.Verify(repo => repo.ObterPorIdAsync(itemId), Times.Once);
    }
}
