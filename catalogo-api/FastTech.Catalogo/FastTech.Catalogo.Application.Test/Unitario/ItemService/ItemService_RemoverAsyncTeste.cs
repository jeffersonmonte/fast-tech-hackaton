using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService
{
    public class ItemService_RemoverAsyncTeste
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<ITipoRefeicaoRepository> _mockTipoRefeicaoRepository;
        private readonly Mock<IEventPublisher> _mockEventPublisher;
        private readonly Services.ItemService _itemService;

        public ItemService_RemoverAsyncTeste()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _mockTipoRefeicaoRepository = new Mock<ITipoRefeicaoRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();
            _itemService = new Services.ItemService(_mockItemRepository.Object, _mockTipoRefeicaoRepository.Object, _mockEventPublisher.Object);
        }

        [Fact]
        public async Task RemoverAsync_ComIdValido_DeveRemoverItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var existente = new Item("Item Teste", "Descrição Teste", new TipoRefeicao("Tipo Teste").Id, new Preco(10.0m));
            _mockItemRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync(existente);

            // Act
            await _itemService.RemoverAsync(itemId);

            // Assert
            _mockItemRepository.Verify(repo => repo.Atualizar(existente), Times.Once);
            _mockItemRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_ComIdInvalido_DeveLancarExcecao()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            _mockItemRepository.Setup(repo => repo.ObterPorIdAsync(itemId)).ReturnsAsync((Item)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.RemoverAsync(itemId));
            _mockItemRepository.Verify(repo => repo.Atualizar(It.IsAny<Item>()), Times.Never);
            _mockItemRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Never);
        }
    }
}
