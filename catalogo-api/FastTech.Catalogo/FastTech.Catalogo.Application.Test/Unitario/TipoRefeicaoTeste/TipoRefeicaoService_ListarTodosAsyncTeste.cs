using FastTech.Catalogo.Domain.Interfaces;
using Moq;

namespace FastTech.Catalogo.Application.Test.Unitario.TipoRefeicaoService
{
    public class TipoRefeicaoService_ListarTodosAsyncTeste
    {
        private readonly Mock<ITipoRefeicaoRepository> _mockRepository;
        private readonly Services.TipoRefeicaoService _service;

        public TipoRefeicaoService_ListarTodosAsyncTeste()
        {
            _mockRepository = new Mock<ITipoRefeicaoRepository>();
            _service = new Services.TipoRefeicaoService(_mockRepository.Object);
        }

        [Fact]
        public async Task ListarTodosAsync_SemTipos_DeveRetornarListaVazia()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.ListarTodosAsync()).ReturnsAsync([]);

            // Act
            var result = await _service.ListarTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
        }
    }
}
