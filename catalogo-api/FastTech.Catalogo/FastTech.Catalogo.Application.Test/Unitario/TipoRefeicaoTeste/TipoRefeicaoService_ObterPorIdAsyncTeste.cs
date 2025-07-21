using System;
using System.Threading.Tasks;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.TipoRefeicaoService;

[Trait("Category", "Unit")]
public class TipoRefeicaoService_ObterPorIdAsyncTeste
{
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockQueryRepository;
    private readonly Services.TipoRefeicaoService _service;

    public TipoRefeicaoService_ObterPorIdAsyncTeste()
    {
        _mockQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _service = new Services.TipoRefeicaoService(_mockQueryRepository.Object);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdValido_DeveRetornarTipo()
    {
        // Arrange
        var tipoId = Guid.NewGuid();
        var tipo = new TipoRefeicao("Tipo Teste") { Id = tipoId };
        _mockQueryRepository.Setup(repo => repo.ObterPorIdAsync(tipoId)).ReturnsAsync(tipo);

        // Act
        var result = await _service.ObterPorIdAsync(tipoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tipoId, result.Id);
        Assert.Equal("Tipo Teste", result.Nome);
        _mockQueryRepository.Verify(repo => repo.ObterPorIdAsync(tipoId), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdInvalido_DeveRetornarNulo()
    {
        // Arrange
        var tipoId = Guid.NewGuid();
        _mockQueryRepository.Setup(repo => repo.ObterPorIdAsync(tipoId)).ReturnsAsync((TipoRefeicao)null!);

        // Act
        var result = await _service.ObterPorIdAsync(tipoId);

        // Assert
        Assert.Null(result);
        _mockQueryRepository.Verify(repo => repo.ObterPorIdAsync(tipoId), Times.Once);
    }
}
