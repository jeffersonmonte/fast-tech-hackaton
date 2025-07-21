using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

[Trait("Category", "Unit")]
public class CardapioService_ObterPorIdAsyncTeste
{
    private readonly Mock<ICardapioQueryRepository> _mockQueryRepo;
    private readonly Mock<ICardapioCommandRepository> _mockCommandRepo;
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepo;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepo;
    private readonly Services.CardapioService _service;

    public CardapioService_ObterPorIdAsyncTeste()
    {
        _mockQueryRepo = new Mock<ICardapioQueryRepository>();
        _mockCommandRepo = new Mock<ICardapioCommandRepository>();
        _mockItemCommandRepo = new Mock<IItemCommandRepository>();
        _mockItemQueryRepo = new Mock<IItemQueryRepository>();

        _service = new Services.CardapioService(
            _mockCommandRepo.Object,
            _mockQueryRepo.Object,
            _mockItemCommandRepo.Object,
            _mockItemQueryRepo.Object
        );
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdValido_DeveRetornarCardapio()
    {
        // Arrange
        var cardapio = new Cardapio("Cardápio Teste", "Descrição Teste", DateTime.UtcNow);

        _mockQueryRepo
            .Setup(repo => repo.ObterPorIdAsync(cardapio.Id))
            .ReturnsAsync(cardapio);

        // Act
        var resultado = await _service.ObterPorIdAsync(cardapio.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(cardapio.Nome, resultado.Nome);
        Assert.Equal(cardapio.Descricao, resultado.Descricao);
        _mockQueryRepo.Verify(repo => repo.ObterPorIdAsync(cardapio.Id), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdInvalido_DeveRetornarNulo()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockQueryRepo
            .Setup(repo => repo.ObterPorIdAsync(id))
            .ReturnsAsync((Cardapio)null!);

        // Act
        var resultado = await _service.ObterPorIdAsync(id);

        // Assert
        Assert.Null(resultado);
        _mockQueryRepo.Verify(repo => repo.ObterPorIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdVazio_DeveLancarExcecao()
    {
        // Arrange
        var id = Guid.Empty;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ObterPorIdAsync(id));
        _mockQueryRepo.Verify(repo => repo.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
    }
}
