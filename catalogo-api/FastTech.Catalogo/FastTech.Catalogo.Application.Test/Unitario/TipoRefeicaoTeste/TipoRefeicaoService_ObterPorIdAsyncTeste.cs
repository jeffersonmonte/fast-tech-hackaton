using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.TipoRefeicaoService
{
    public class TipoRefeicaoService_ObterPorIdAsyncTeste
    {
        private readonly Mock<ITipoRefeicaoRepository> _mockRepository;
        private readonly Services.TipoRefeicaoService _service;

        public TipoRefeicaoService_ObterPorIdAsyncTeste()
        {
            _mockRepository = new Mock<ITipoRefeicaoRepository>();
            _service = new Services.TipoRefeicaoService(_mockRepository.Object);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComIdValido_DeveRetornarTipo()
        {
            // Arrange
            var tipoId = Guid.NewGuid();
            var tipo = new TipoRefeicao("Tipo Teste") { Id = tipoId };
            _mockRepository.Setup(repo => repo.ObterPorIdAsync(tipoId)).ReturnsAsync(tipo);

            // Act
            var result = await _service.ObterPorIdAsync(tipoId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tipoId, result.Id);
            Assert.Equal("Tipo Teste", result.Nome);
            _mockRepository.Verify(repo => repo.ObterPorIdAsync(tipoId), Times.Once);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComIdInvalido_DeveRetornarNulo()
        {
            // Arrange
            var tipoId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.ObterPorIdAsync(tipoId)).ReturnsAsync((TipoRefeicao)null!);

            // Act
            var result = await _service.ObterPorIdAsync(tipoId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.ObterPorIdAsync(tipoId), Times.Once);
        }
    }
}
