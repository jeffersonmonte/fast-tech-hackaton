using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Query;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService;

[Trait("Category", "Unit")]
public class ItemService_ListarTodosAsyncTeste
{
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepository;
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockTipoRefeicaoQueryRepository;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly FastTech.Catalogo.Application.Services.ItemService _itemService;

    public ItemService_ListarTodosAsyncTeste()
    {
        _mockItemQueryRepository = new Mock<IItemQueryRepository>();
        _mockTipoRefeicaoQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _mockEventPublisher = new Mock<IEventPublisher>();

        _itemService = new FastTech.Catalogo.Application.Services.ItemService(
            itemCommandRepository: null!, // comando não usado
            _mockItemQueryRepository.Object,
            _mockTipoRefeicaoQueryRepository.Object,
            _mockEventPublisher.Object
        );
    }

    [Fact]
    public async Task ListarTodosAsync_ComItens_DeveRetornarListaDeItens()
    {
        // Arrange
        var itens = new List<Item>
        {
            new("Item 1", "Descrição 1", Guid.NewGuid(), new Preco(10.0m)),
            new("Item 2", "Descrição 2", Guid.NewGuid(), new Preco(20.0m))
        };

        _mockItemQueryRepository
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync(itens);

        // Act
        var result = await _itemService.ListarTodosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(itens.Count, result.Count());
        Assert.Collection(result,
            item => Assert.Equal(itens[0].Id, item.Id),
            item => Assert.Equal(itens[1].Id, item.Id));

        _mockItemQueryRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ListarTodosAsync_SemItens_DeveRetornarListaVazia()
    {
        // Arrange
        _mockItemQueryRepository
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync([]);

        // Act
        var result = await _itemService.ListarTodosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockItemQueryRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }
}
