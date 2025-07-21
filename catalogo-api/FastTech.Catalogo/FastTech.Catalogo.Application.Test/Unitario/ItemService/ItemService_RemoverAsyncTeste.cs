using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService;

[Trait("Category", "Unit")]
public class ItemService_RemoverAsyncTeste
{
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepository;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepository;
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockTipoRefeicaoQueryRepository;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly Services.ItemService _itemService;

    public ItemService_RemoverAsyncTeste()
    {
        _mockItemCommandRepository = new Mock<IItemCommandRepository>();
        _mockItemQueryRepository = new Mock<IItemQueryRepository>();
        _mockTipoRefeicaoQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _mockEventPublisher = new Mock<IEventPublisher>();

        _itemService = new Services.ItemService(
            _mockItemCommandRepository.Object,
            _mockItemQueryRepository.Object,
            _mockTipoRefeicaoQueryRepository.Object,
            _mockEventPublisher.Object
        );
    }

    [Fact]
    public async Task RemoverAsync_ComIdValido_DeveRemoverItem()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var existente = new Item("Item Teste", "Descrição Teste", Guid.NewGuid(), new Preco(10.0m));
        _mockItemCommandRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync(existente);

        // Act
        await _itemService.RemoverAsync(itemId);

        // Assert
        _mockItemCommandRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task RemoverAsync_ComIdInvalido_DeveLancarExcecao()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        _mockItemCommandRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync((Item)null!);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.RemoverAsync(itemId));

        _mockItemCommandRepository.Verify(repo => repo.Remover(It.IsAny<Item>()), Times.Never);
        _mockItemCommandRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Never);
    }
}
