using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Services;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.ValueObjects;
using Moq;
using Xunit;

namespace FastTech.Catalogo.Application.Test.Unitario.ItemService
{
    public class ItemService_ListarTodosAsyncTeste
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<ITipoRefeicaoRepository> _mockTipoRefeicaoRepository;
        private readonly Services.ItemService _itemService;

        public ItemService_ListarTodosAsyncTeste()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _mockTipoRefeicaoRepository = new Mock<ITipoRefeicaoRepository>();
            _itemService = new Services.ItemService(_mockItemRepository.Object, _mockTipoRefeicaoRepository.Object);
        }

        [Fact]
        public async Task ListarTodosAsync_ComItens_DeveRetornarListaDeItens()
        {
            // Arrange
            var itens = new List<Item>
            {
                new("Item 1", "Descrição 1", new TipoRefeicao("Tipo 1").Id, new Preco(10.0m)),
                new("Item 2", "Descrição 2", new TipoRefeicao("Tipo 2").Id, new Preco(20.0m))
            };

            _mockItemRepository.Setup(repo => repo.ListarTodosAsync()).ReturnsAsync(itens);

            // Act
            var result = await _itemService.ListarTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(itens.Count, result.Count());
            Assert.Collection(result,
                item =>
                {
                    Assert.Equal(itens[0].Id, item.Id);
                },
                item =>
                {
                    Assert.Equal(itens[1].Id, item.Id);
                });
            _mockItemRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
        }

        [Fact]
        public async Task ListarTodosAsync_SemItens_DeveRetornarListaVazia()
        {
            // Arrange
            _mockItemRepository.Setup(repo => repo.ListarTodosAsync()).ReturnsAsync([]);

            // Act
            var result = await _itemService.ListarTodosAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockItemRepository.Verify(repo => repo.ListarTodosAsync(), Times.Once);
        }
    }
}
