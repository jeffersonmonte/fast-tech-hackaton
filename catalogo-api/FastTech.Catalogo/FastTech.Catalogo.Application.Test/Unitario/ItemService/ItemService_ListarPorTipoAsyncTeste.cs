﻿using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService
{
    public class ItemService_ListarPorTipoAsyncTeste
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<ITipoRefeicaoRepository> _mockTipoRefeicaoRepository;
        private readonly Mock<IEventPublisher> _mockEventPublisher;
        private readonly Services.ItemService _itemService;

        public ItemService_ListarPorTipoAsyncTeste()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _mockTipoRefeicaoRepository = new Mock<ITipoRefeicaoRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();
            _itemService = new Services.ItemService(_mockItemRepository.Object, _mockTipoRefeicaoRepository.Object, _mockEventPublisher.Object);
        }

        [Fact]
        public async Task ListarPorTipoAsync_ComTipoValido_DeveRetornarItens()
        {
            // Arrange
            var tipoId = Guid.NewGuid();
            var itens = new List<Item>
            {
                new("Item 1", "Descrição 1", new TipoRefeicao("Tipo 1").Id, new Preco(10.0m))
            };
            _mockItemRepository.Setup(repo => repo.ListarPorTipoAsync(tipoId)).ReturnsAsync(itens);

            // Act
            var result = await _itemService.ListarPorTipoAsync(tipoId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockItemRepository.Verify(repo => repo.ListarPorTipoAsync(tipoId), Times.Once);
        }

        [Fact]
        public async Task ListarPorTipoAsync_ComTipoInvalido_DeveRetornarListaVazia()
        {
            // Arrange
            var tipoId = Guid.NewGuid();
            _mockItemRepository.Setup(repo => repo.ListarPorTipoAsync(tipoId)).ReturnsAsync([]);

            // Act
            var result = await _itemService.ListarPorTipoAsync(tipoId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockItemRepository.Verify(repo => repo.ListarPorTipoAsync(tipoId), Times.Once);
        }
    }
}
