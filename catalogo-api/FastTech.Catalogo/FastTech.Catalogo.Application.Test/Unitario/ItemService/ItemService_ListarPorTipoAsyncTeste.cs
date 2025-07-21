using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Query;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService;

[Trait("Category", "Unit")]
public class ItemService_ListarPorTipoAsyncTeste
{
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepository;
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockTipoRefeicaoQueryRepository;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly FastTech.Catalogo.Application.Services.ItemService _itemService;

    public ItemService_ListarPorTipoAsyncTeste()
    {
        _mockItemQueryRepository = new Mock<IItemQueryRepository>();
        _mockTipoRefeicaoQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _mockEventPublisher = new Mock<IEventPublisher>();

        _itemService = new FastTech.Catalogo.Application.Services.ItemService(
            itemCommandRepository: null!, // Não é usado neste caso
            _mockItemQueryRepository.Object,
            _mockTipoRefeicaoQueryRepository.Object,
            _mockEventPublisher.Object
        );
    }

    [Fact]
    public async Task ListarPorTipoAsync_ComTipoValido_DeveRetornarItens()
    {
        // Arrange
        var tipoId = Guid.NewGuid();
        var itens = new List<Item>
        {
            new("Item 1", "Descrição 1", tipoId, new Preco(10.0m))
        };

        _mockItemQueryRepository
            .Setup(repo => repo.ListarPorTipoAsync(tipoId))
            .ReturnsAsync(itens);

        // Act
        var result = await _itemService.ListarPorTipoAsync(tipoId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _mockItemQueryRepository.Verify(repo => repo.ListarPorTipoAsync(tipoId), Times.Once);
    }

    [Fact]
    public async Task ListarPorTipoAsync_ComTipoInvalido_DeveRetornarListaVazia()
    {
        // Arrange
        var tipoId = Guid.NewGuid();

        _mockItemQueryRepository
            .Setup(repo => repo.ListarPorTipoAsync(tipoId))
            .ReturnsAsync([]);

        // Act
        var result = await _itemService.ListarPorTipoAsync(tipoId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockItemQueryRepository.Verify(repo => repo.ListarPorTipoAsync(tipoId), Times.Once);
    }
}
