using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Query;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.TipoRefeicaoService;

[Trait("Category", "Unit")]
public class TipoRefeicaoService_ListarTodosAsyncTeste
{
    private readonly Mock<ITipoRefeicaoQueryRepository> _mockQueryRepository;
    private readonly Services.TipoRefeicaoService _service;

    public TipoRefeicaoService_ListarTodosAsyncTeste()
    {
        _mockQueryRepository = new Mock<ITipoRefeicaoQueryRepository>();
        _service = new Services.TipoRefeicaoService(_mockQueryRepository.Object);
    }

    [Fact]
    public async Task ListarTodosAsync_SemTipos_DeveRetornarListaVazia()
    {
        // Arrange
        _mockQueryRepository.Setup(repo => repo.ListarTodosAsync()).ReturnsAsync([]);

        // Act
        var result = await _service.ListarTodosAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockQueryRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ListarTodosAsync_ComTipos_DeveRetornarListaPreenchida()
    {
        // Arrange
        var tipos = new List<TipoRefeicao>
        {
            new("Café da Manhã"),
            new("Almoço")
        };

        _mockQueryRepository.Setup(repo => repo.ListarTodosAsync()).ReturnsAsync(tipos);

        // Act
        var result = (await _service.ListarTodosAsync()).ToList();

        // Assert
        Assert.Equal(tipos.Count, result.Count);
        Assert.Equal(tipos[0].Nome, result[0].Nome);
        Assert.Equal(tipos[1].Nome, result[1].Nome);
        _mockQueryRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
    }
}
