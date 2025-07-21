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
public class ItemService_AtualizarAsyncTeste
{
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepository;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepository;
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockTipoRefeicaoQueryRepository;
    private readonly Mock<IEventPublisher> _mockEventPublisher;
    private readonly FastTech.Catalogo.Application.Services.ItemService _itemService;

    public ItemService_AtualizarAsyncTeste()
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
    public async Task AtualizarAsync_ComDadosValidos_DeveAtualizarItem()
    {
        // Arrange
        var tipo = new TipoRefeicao("Tipo Atualizado");
        var existente = new Item("Item Teste", "Descrição Teste", tipo.Id, new Preco(10.0m));
        var dto = new ItemUpdateDto
        {
            Id = existente.Id,
            Nome = "Item Atualizado",
            Descricao = "Descrição Atualizada",
            TipoRefeicaoId = Guid.NewGuid(),
            Valor = 20.0m
        };

        _mockItemCommandRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(existente);
        _mockItemQueryRepository.Setup(r => r.ObterPorNomeAsync(dto.Nome)).ReturnsAsync((Item)null!);
        _mockTipoRefeicaoQueryRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(tipo);
        _mockItemCommandRepository.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        // Act
        await _itemService.AtualizarAsync(dto);

        // Assert
        _mockItemCommandRepository.Verify(r => r.SalvarAlteracoesAsync(), Times.Exactly(2));
    }

    [Fact]
    public async Task AtualizarAsync_ComItemInexistente_DeveLancarExcecao()
    {
        // Arrange
        var dto = new ItemUpdateDto
        {
            Id = Guid.NewGuid(),
            Nome = "Item Atualizado",
            Descricao = "Descrição Atualizada",
            TipoRefeicaoId = Guid.NewGuid(),
            Valor = 20.0m
        };

        _mockItemQueryRepository.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync((Item)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _itemService.AtualizarAsync(dto));
    }

    [Fact]
    public async Task AtualizarAsync_ComNomeDuplicado_DeveLancarExcecao()
    {
        // Arrange
        var dto = new ItemUpdateDto
        {
            Id = Guid.NewGuid(),
            Nome = "Item Duplicado",
            Descricao = "Descrição Atualizada",
            TipoRefeicaoId = Guid.NewGuid(),
            Valor = 20.0m
        };

        var tipo = new TipoRefeicao("Tipo Atualizado");
        var existente = new Item("Item Original", "Descrição Original", tipo.Id, new Preco(10.0m));

        _mockItemCommandRepository.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(existente);
        _mockItemQueryRepository.Setup(r => r.ObterPorNomeAsync(dto.Nome)).ReturnsAsync(new Item("Item Duplicado", "Desc", tipo.Id, new Preco(9)));
        _mockTipoRefeicaoQueryRepository.Setup(r => r.ObterPorIdAsync(dto.TipoRefeicaoId)).ReturnsAsync(tipo);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.AtualizarAsync(dto));
    }
}
