using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

[Trait("Category", "Unit")]
public class CardapioService_ObterTodosAsyncTeste
{
    private readonly Mock<ICardapioQueryRepository> _mockQueryRepo;
    private readonly Mock<ICardapioCommandRepository> _mockCommandRepo;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepo;
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepo;
    private readonly Services.CardapioService _service;

    public CardapioService_ObterTodosAsyncTeste()
    {
        _mockQueryRepo = new Mock<ICardapioQueryRepository>();
        _mockCommandRepo = new Mock<ICardapioCommandRepository>();
        _mockItemQueryRepo = new Mock<IItemQueryRepository>();
        _mockItemCommandRepo = new Mock<IItemCommandRepository>();

        _service = new Services.CardapioService(
            _mockCommandRepo.Object,
            _mockQueryRepo.Object,
            _mockItemCommandRepo.Object,
            _mockItemQueryRepo.Object
        );
    }

    [Fact]
    public async Task ObterTodosAsync_ComCardapiosExistentes_DeveRetornarListaDeCardapios()
    {
        // Arrange
        var cardapios = new List<Cardapio>
        {
            new("Cardapio 1", "Descricao 1", DateTime.UtcNow),
            new("Cardapio 2", "Descricao 2", DateTime.UtcNow)
        };

        _mockQueryRepo
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync(cardapios);

        // Act
        var resultado = (await _service.ObterTodosAsync()).ToList();

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("Cardapio 1", resultado[0].Nome);
        Assert.Equal("Cardapio 2", resultado[1].Nome);
        _mockQueryRepo.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAsync_SemCardapios_DeveRetornarListaVazia()
    {
        // Arrange
        _mockQueryRepo
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync([]);

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
        _mockQueryRepo.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ObterTodosAsync_ComCardapiosSemItens_DeveRetornarListaDeCardapiosVazios()
    {
        // Arrange
        var cardapios = new List<Cardapio>
        {
            new("Cardapio 1", "Descricao 1", DateTime.UtcNow),
            new("Cardapio 2", "Descricao 2", DateTime.UtcNow)
        };

        _mockQueryRepo
            .Setup(repo => repo.ListarTodosAsync())
            .ReturnsAsync(cardapios);

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        Assert.NotNull(resultado);
        Assert.All(resultado, c => Assert.Empty(c.Itens!));
        _mockQueryRepo.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }
}
