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
    public class ItemService_AdicionarAsyncTeste
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<ITipoRefeicaoRepository> _mockTipoRefeicaoRepository;
        private readonly Services.ItemService _itemService;

        public ItemService_AdicionarAsyncTeste()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _mockTipoRefeicaoRepository = new Mock<ITipoRefeicaoRepository>();
            _itemService = new Services.ItemService(_mockItemRepository.Object, _mockTipoRefeicaoRepository.Object);
        }

        [Fact]
        public async Task AdicionarAsync_ComDadosValidos_DeveAdicionarItem()
        {
            // Arrange
            var tipo = new TipoRefeicao("Tipo Teste");
            var dto = new ItemInputDto
            {
                Nome = "Item Teste",
                Descricao = "Descrição Teste",
                TipoRefeicaoId = Guid.NewGuid(),
                Valor = 10.0m
            };
            _mockTipoRefeicaoRepository.Setup(repo => repo.ObterPorIdAsync(dto.TipoRefeicaoId)).ReturnsAsync(tipo);
            _mockItemRepository.Setup(repo => repo.AdicionarAsync(It.IsAny<Item>())).Returns(Task.CompletedTask);

            // Act
            var result = await _itemService.AdicionarAsync(dto);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _mockItemRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Item>()), Times.Once);
            _mockItemRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        public async Task AdicionarAsync_ComTipoInvalido_DeveLancarExcecao()
        {
            // Arrange
            var dto = new ItemInputDto
            {
                Nome = "Item Teste",
                Descricao = "Descrição Teste",
                TipoRefeicaoId = Guid.NewGuid(),
                Valor = 10.0m
            };
            _mockTipoRefeicaoRepository.Setup(repo => repo.ObterPorIdAsync(dto.TipoRefeicaoId)).ReturnsAsync((TipoRefeicao)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.AdicionarAsync(dto));
            _mockItemRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Item>()), Times.Never);
        }

        [Fact]
        public async Task AdicionarAsync_ComNomeDuplicado_DeveLancarExcecao()
        {
            // Arrange
            var tipo = new TipoRefeicao("Tipo Teste");
            var itemDuplicado = new Item("Item Duplicado", "Descrição Teste", tipo.Id, new Preco(10.0m));
            var dto = new ItemInputDto
            {
                Nome = "Item Duplicado",
                Descricao = "Descrição Teste",
                TipoRefeicaoId = itemDuplicado.TipoRefeicaoId,
                Valor = 10.0m
            };

            _mockTipoRefeicaoRepository.Setup(repo => repo.ObterPorIdAsync(dto.TipoRefeicaoId)).ReturnsAsync(tipo);
            _mockItemRepository.Setup(repo => repo.ObterPorNomeAsync(dto.Nome)).ReturnsAsync(itemDuplicado);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.AdicionarAsync(dto));
            _mockItemRepository.Verify(repo => repo.AdicionarAsync(It.IsAny<Item>()), Times.Never);
        }
    }
}
