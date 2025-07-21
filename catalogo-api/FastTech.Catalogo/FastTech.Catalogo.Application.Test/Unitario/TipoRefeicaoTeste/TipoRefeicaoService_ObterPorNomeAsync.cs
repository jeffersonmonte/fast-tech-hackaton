using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using Moq;

namespace FastTech.Catalogo.Application.Test.Unitario.TipoRefeicaoService
{
    public class TipoRefeicaoService_ObterPorNomeAsync
    {
        private readonly Mock<ITipoRefeicaoRepository> _mockRepository;
        private readonly Services.TipoRefeicaoService _service;

        public TipoRefeicaoService_ObterPorNomeAsync()
        {
            _mockRepository = new Mock<ITipoRefeicaoRepository>();
            _service = new Services.TipoRefeicaoService(_mockRepository.Object);
        }

        [Fact]
        public async Task ObterPorNomeAsync_ComNomeValido_DeveRetornarTipo()
        {
            // Arrange
            var nome = "Tipo Teste";
            var tipo = new TipoRefeicao(nome);
            _mockRepository.Setup(repo => repo.ObterPorNomeAsync(nome)).ReturnsAsync(tipo);

            // Act
            var result = await _service.ObterPorNomeAsync(nome);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nome, result.Nome);
            _mockRepository.Verify(repo => repo.ObterPorNomeAsync(nome), Times.Once);
        }

        [Fact]
        public async Task ObterPorNomeAsync_ComNomeInvalido_DeveRetornarNulo()
        {
            // Arrange
            var nome = "Tipo Inexistente";
            _mockRepository.Setup(repo => repo.ObterPorNomeAsync(nome)).ReturnsAsync((TipoRefeicao)null!);

            // Act
            var result = await _service.ObterPorNomeAsync(nome);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.ObterPorNomeAsync(nome), Times.Once);
        }
    }
}
