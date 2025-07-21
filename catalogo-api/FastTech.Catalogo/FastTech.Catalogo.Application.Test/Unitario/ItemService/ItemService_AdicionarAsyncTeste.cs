using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
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
public class ItemService_AdicionarAsyncTeste
{
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepository;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepository;
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockTipoRefeicaoQueryRepository;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly FastTech.Catalogo.Application.Services.ItemService _itemService;

    public ItemService_AdicionarAsyncTeste()
    {
        _mockItemCommandRepository = new Mock<IItemCommandRepository>();
        _mockItemQueryRepository = new Mock<IItemQueryRepository>();
        _mockTipoRefeicaoQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _mockEventPublisher = new Mock<IEventPublisher>();

        _itemService = new FastTech.Catalogo.Application.Services.ItemService(
            _mockItemCommandRepository.Object,
            _mockItemQueryRepository.Object,
            _mockTipoRefeicaoQueryRepository.Object,
            _mockEventPublisher.Object
        );
    }

    [Fact]
    public async Task AdicionarAsync_ComDadosValidos_DeveAdicionarItem()
    {
        // Arrange
        var tipo = new TipoRefeicao("Tipo Teste");
        var dto = new ItemInputDto
        {
            Nome = "Item Teste",
            Descricao = "Descrição Teste",
            TipoRefeicaoId = Guid.NewGuid(),
            Valor = 10.0m
        };

        _mockTipoRefeicaoQueryRepository
            .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(tipo);

        _mockItemQueryRepository
            .Setup(r => r.ObterPorNomeAsync(dto.Nome))
            .ReturnsAsync((Item)null!);

        _mockItemCommandRepository
            .Setup(r => r.AdicionarAsync(It.IsAny<Item>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _itemService.AdicionarAsync(dto);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _mockItemCommandRepository.Verify(r => r.AdicionarAsync(It.IsAny<Item>()), Times.Once);
        _mockItemCommandRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task AdicionarAsync_ComTipoInvalido_DeveLancarExcecao()
    {
        // Arrange
        var dto = new ItemInputDto
        {
            Nome = "Item Teste",
            Descricao = "Descrição Teste",
            TipoRefeicaoId = Guid.NewGuid(),
            Valor = 10.0m
        };

        _mockTipoRefeicaoQueryRepository
            .Setup(r => r.ObterPorIdAsync(dto.TipoRefeicaoId))
            .ReturnsAsync((TipoRefeicao)null!);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.AdicionarAsync(dto));
        _mockItemCommandRepository.Verify(r => r.AdicionarAsync(It.IsAny<Item>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarAsync_ComNomeDuplicado_DeveLancarExcecao()
    {
        // Arrange
        var tipo = new TipoRefeicao("Tipo Teste");
        var itemExistente = new Item("Item Duplicado", "Descrição Teste", tipo.Id, new Preco(10.0m));
        var dto = new ItemInputDto
        {
            Nome = "Item Duplicado",
            Descricao = "Descrição Teste",
            TipoRefeicaoId = tipo.Id,
            Valor = 10.0m
        };

        _mockTipoRefeicaoQueryRepository
            .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(tipo);

        _mockItemQueryRepository
            .Setup(r => r.ObterPorNomeAsync(dto.Nome))
            .ReturnsAsync(itemExistente);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.AdicionarAsync(dto));
        _mockItemCommandRepository.Verify(r => r.AdicionarAsync(It.IsAny<Item>()), Times.Never);
    }
}
