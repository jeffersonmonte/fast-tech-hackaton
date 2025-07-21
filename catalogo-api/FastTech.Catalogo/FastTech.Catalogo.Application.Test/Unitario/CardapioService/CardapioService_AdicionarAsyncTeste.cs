using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

[Trait("Category", "Unit")]
public class CardapioService_AdicionarAsyncTeste
{
    private readonly Mock<ICardapioCommandRepository> _mockCommandRepo;
    private readonly Mock<ICardapioQueryRepository> _mockQueryRepo;
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepo;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepo;

    private readonly FastTech.Catalogo.Application.Services.CardapioService _service;

    public CardapioService_AdicionarAsyncTeste()
    {
        _mockCommandRepo = new Mock<ICardapioCommandRepository>();
        _mockQueryRepo = new Mock<ICardapioQueryRepository>();
        _mockItemCommandRepo = new Mock<IItemCommandRepository>();
        _mockItemQueryRepo = new Mock<IItemQueryRepository>();

        _service = new FastTech.Catalogo.Application.Services.CardapioService(
            _mockCommandRepo.Object,
            _mockQueryRepo.Object,
            _mockItemCommandRepo.Object,
            _mockItemQueryRepo.Object);
    }

    [Fact]
    public async Task AdicionarCardapio_ComGuidsExistentes_DeveAdicionarComSucesso()
    {
        var item = new Item("Nome", "Descricao", Guid.NewGuid(), new(20));
        var dto = new CardapioInputDto
        {
            Nome = "Novo Cardápio",
            Descricao = "Desc",
            ItensIds = [item.Id]
        };

        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>()))
                            .ReturnsAsync([item]);

        _mockQueryRepo.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cardapio, bool>>>()))
                      .ReturnsAsync(false);

        _mockCommandRepo.Setup(r => r.AdicionarAsync(It.IsAny<Cardapio>()))
                        .Returns(Task.CompletedTask);

        // Act
        var id = await _service.AdicionarAsync(dto);

        // Assert
        _mockCommandRepo.Verify(r => r.AdicionarAsync(It.IsAny<Cardapio>()), Times.Once);
        Assert.NotEqual(Guid.Empty, id);
    }

    [Fact]
    public async Task AdicionarCardapio_ComItensNaoExistentes_DeveLancarExcecao()
    {
        var dto = new CardapioInputDto
        {
            Nome = "Nome",
            Descricao = "Desc",
            ItensIds = [Guid.NewGuid()]
        };

        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>()))
                            .ReturnsAsync([]);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AdicionarAsync(dto));
        _mockCommandRepo.Verify(r => r.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_SemItens_DeveLancarExcecao()
    {
        var dto = new CardapioInputDto
        {
            Nome = "Cardápio",
            Descricao = "Desc",
            ItensIds = []
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AdicionarAsync(dto));
        _mockCommandRepo.Verify(r => r.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_ComNomeDuplicado_DeveLancarExcecao()
    {
        var item = new Item("Nome", "Descricao", Guid.NewGuid(), new(20));
        var dto = new CardapioInputDto
        {
            Nome = "Duplicado",
            Descricao = "Desc",
            ItensIds = [item.Id]
        };

        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>()))
                            .ReturnsAsync([item]);

        _mockQueryRepo.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cardapio, bool>>>()))
                      .ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AdicionarAsync(dto));
        _mockCommandRepo.Verify(r => r.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_ComNomeVazio_DeveLancarExcecao()
    {
        var item = new Item("Nome", "Descricao", Guid.NewGuid(), new(20));
        var dto = new CardapioInputDto
        {
            Nome = "",
            Descricao = "Desc",
            ItensIds = [item.Id]
        };

        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>()))
                            .ReturnsAsync([item]);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AdicionarAsync(dto));
        _mockCommandRepo.Verify(r => r.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }

    [Fact]
    public async Task AdicionarCardapio_ComItensParcialmenteExistentes_DeveLancarExcecao()
    {
        var existente = new Item("Nome", "Descricao", Guid.NewGuid(), new(20));
        var inexistente = Guid.NewGuid();

        var dto = new CardapioInputDto
        {
            Nome = "Novo",
            Descricao = "Desc",
            ItensIds = [existente.Id, inexistente]
        };

        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>()))
                            .ReturnsAsync([existente]);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AdicionarAsync(dto));
        _mockCommandRepo.Verify(r => r.AdicionarAsync(It.IsAny<Cardapio>()), Times.Never);
    }
}
