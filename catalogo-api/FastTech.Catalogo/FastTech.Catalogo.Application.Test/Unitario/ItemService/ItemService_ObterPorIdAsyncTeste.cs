using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService
{
    public class ItemService_ObterPorIdAsyncTeste
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<ITipoRefeicaoRepository> _mockTipoRefeicaoRepository;
        private readonly Mock<IEventPublisher> _mockEventPublisher;
        private readonly Services.ItemService _itemService;

        public ItemService_ObterPorIdAsyncTeste()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _mockTipoRefeicaoRepository = new Mock<ITipoRefeicaoRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();
            _itemService = new Services.ItemService(_mockItemRepository.Object, _mockTipoRefeicaoRepository.Object, _mockEventPublisher.Object);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComIdValido_DeveRetornarItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var item = new Item("Item Teste", "Descrição Teste", new TipoRefeicao("Tipo Teste").Id, new Preco(10.0m));
            _mockItemRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync(item);

            // Act
            var result = await _itemService.ObterPorIdAsync(itemId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(item.Nome, result.Nome);
            _mockItemRepository.Verify(repo => repo.ObterPorIdAsync(itemId), Times.Once);
        }

        [Fact]
        public async Task ObterPorIdAsync_ComIdInvalido_DeveRetornarNulo()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            _mockItemRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync((Item)null!);

            // Act
            var result = await _itemService.ObterPorIdAsync(itemId);

            // Assert
            Assert.Null(result);
            _mockItemRepository.Verify(repo => repo.ObterPorIdAsync(itemId), Times.Once);
        }
    }
}
