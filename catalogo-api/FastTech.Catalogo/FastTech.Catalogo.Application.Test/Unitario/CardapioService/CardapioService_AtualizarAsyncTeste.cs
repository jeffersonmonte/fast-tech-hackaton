using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

[Trait("Category", "Unit")]
public class CardapioService_AtualizarAsyncTeste
{
    private readonly Mock<ICardapioCommandRepository> _mockCommandRepo;
    private readonly Mock<ICardapioQueryRepository> _mockQueryRepo;
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepo;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepo;

    private readonly FastTech.Catalogo.Application.Services.CardapioService _service;

    public CardapioService_AtualizarAsyncTeste()
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
    public async Task AtualizarCardapio_ComNovoNomeEDescricao_DeveAtualizarComSucesso()
    {
        var item = new Item("Nome", "Descricao", Guid.NewGuid(), new(20));
        var cardapio = new Cardapio("Antigo", "Desc", DateTime.UtcNow);
        var dto = new CardapioUpdateDto
        {
            Id = cardapio.Id,
            Nome = "Atualizado",
            Descricao = "Nova desc",
            ItensIds = [item.Id]
        };

        _mockCommandRepo.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync(cardapio);
        _mockQueryRepo.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cardapio, bool>>>())).ReturnsAsync(false);
        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>())).ReturnsAsync([item]);
        _mockCommandRepo.Setup(r => r.LimparItensESalvarAsync(dto.Id)).Returns(Task.CompletedTask);
        _mockCommandRepo.Setup(r => r.SalvarAlteracoesAsync()).Returns(Task.CompletedTask);

        await _service.AtualizarAsync(dto);

        _mockCommandRepo.Verify(r => r.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task AtualizarCardapio_ComIdInexistente_DeveLancarExcecao()
    {
        var dto = new CardapioUpdateDto
        {
            Id = Guid.NewGuid(),
            Nome = "Atualizado",
            Descricao = "Nova desc",
            ItensIds = [Guid.NewGuid()]
        };

        _mockCommandRepo.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync((Cardapio)null!);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AtualizarAsync(dto));
        _mockCommandRepo.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }

    [Fact]
    public async Task AtualizarCardapio_ComItensNaoExistentes_DeveLancarExcecao()
    {
        var cardapio = new Cardapio("Antigo", "Desc", DateTime.UtcNow);
        var dto = new CardapioUpdateDto
        {
            Id = cardapio.Id,
            Nome = "Atualizado",
            Descricao = "Nova desc",
            ItensIds = [Guid.NewGuid()]
        };

        _mockCommandRepo.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync(cardapio);
        _mockQueryRepo.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cardapio, bool>>>())).ReturnsAsync(false);
        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>())).ReturnsAsync([]);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AtualizarAsync(dto));
        _mockCommandRepo.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }

    [Fact]
    public async Task AtualizarCardapio_ComNomeDuplicado_DeveLancarExcecao()
    {
        var item = new Item("Nome", "Descricao", Guid.NewGuid(), new(20));
        var cardapio = new Cardapio("Antigo", "Desc", DateTime.UtcNow);
        var dto = new CardapioUpdateDto
        {
            Id = cardapio.Id,
            Nome = "Duplicado",
            Descricao = "Nova desc",
            ItensIds = [item.Id]
        };

        _mockCommandRepo.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync(cardapio);
        _mockQueryRepo.Setup(r => r.ExisteAsync(It.IsAny<Expression<Func<Cardapio, bool>>>())).ReturnsAsync(true);
        _mockItemCommandRepo.Setup(r => r.ListarAsync(It.IsAny<Expression<Func<Item, bool>>>())).ReturnsAsync([item]);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AtualizarAsync(dto));
        _mockCommandRepo.Verify(r => r.SalvarAlteracoesAsync(), Times.Never);
    }
}
