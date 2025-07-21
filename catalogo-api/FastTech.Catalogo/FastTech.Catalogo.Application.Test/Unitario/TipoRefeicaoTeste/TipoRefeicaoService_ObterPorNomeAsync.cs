using System.Threading.Tasks;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.TipoRefeicaoService;

[Trait("Category", "Unit")]
public class TipoRefeicaoService_ObterPorNomeAsync
{
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockQueryRepository;
    private readonly Services.TipoRefeicaoService _service;

    public TipoRefeicaoService_ObterPorNomeAsync()
    {
        _mockQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _service = new Services.TipoRefeicaoService(_mockQueryRepository.Object);
    }

    [Fact]
    public async Task ObterPorNomeAsync_ComNomeValido_DeveRetornarTipo()
    {
        // Arrange
        var nome = "Tipo Teste";
        var tipo = new TipoRefeicao(nome);
        _mockQueryRepository.Setup(repo => repo.ObterPorNomeAsync(nome)).ReturnsAsync(tipo);

        // Act
        var result = await _service.ObterPorNomeAsync(nome);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nome, result.Nome);
        _mockQueryRepository.Verify(repo => repo.ObterPorNomeAsync(nome), Times.Once);
    }

    [Fact]
    public async Task ObterPorNomeAsync_ComNomeInvalido_DeveRetornarNulo()
    {
        // Arrange
        var nome = "Tipo Inexistente";
        _mockQueryRepository.Setup(repo => repo.ObterPorNomeAsync(nome)).ReturnsAsync((TipoRefeicao)null!);

        // Act
        var result = await _service.ObterPorNomeAsync(nome);

        // Assert
        Assert.Null(result);
        _mockQueryRepository.Verify(repo => repo.ObterPorNomeAsync(nome), Times.Once);
    }
}
