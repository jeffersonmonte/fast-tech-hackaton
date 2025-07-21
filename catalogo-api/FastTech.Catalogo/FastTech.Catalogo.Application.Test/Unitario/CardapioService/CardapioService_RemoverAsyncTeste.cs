using System;
using System.Threading.Tasks;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.CardapioService;

[Trait("Category", "Unit")]
public class CardapioService_RemoverTeste
{
    private readonly Mock<ICardapioQueryRepository> _mockQueryRepo;
    private readonly Mock<ICardapioCommandRepository> _mockCommandRepo;
    private readonly Mock<IItemQueryRepository> _mockItemQueryRepo;
    private readonly Mock<IItemCommandRepository> _mockItemCommandRepo;
    private readonly Services.CardapioService _service;

    public CardapioService_RemoverTeste()
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
    public async Task Remover_ComIdValido_DeveRemoverComSucesso()
    {
        // Arrange
        var cardapio = new Cardapio("Cardápio Teste", "Descrição Teste", DateTime.UtcNow);

        _mockCommandRepo
            .Setup(repo => repo.ObterPorIdAsync(cardapio.Id))
            .ReturnsAsync(cardapio);

        // Act
        await _service.RemoverAsync(cardapio.Id);

        // Assert
        _mockCommandRepo.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
    }

    [Fact]
    public async Task Remover_ComIdInvalido_DeveLancarExcecao()
    {
        // Arrange
        var idInvalido = Guid.NewGuid();

        _mockCommandRepo
            .Setup(repo => repo.ObterPorIdAsync(idInvalido))
            .ReturnsAsync((Cardapio)null!);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RemoverAsync(idInvalido));
        _mockCommandRepo.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Never);
    }
}
