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
    public class ItemService_AtualizarAsyncTeste
    {
        private readonly Mock<IItemRepository> _mockItemRepository;
        private readonly Mock<ITipoRefeicaoRepository> _mockTipoRefeicaoRepository;
        private readonly Services.ItemService _itemService;

        public ItemService_AtualizarAsyncTeste()
        {
            _mockItemRepository = new Mock<IItemRepository>();
            _mockTipoRefeicaoRepository = new Mock<ITipoRefeicaoRepository>();
            _itemService = new Services.ItemService(_mockItemRepository.Object, _mockTipoRefeicaoRepository.Object);
        }

        [Fact]
        public async Task AtualizarAsync_ComDadosValidos_DeveAtualizarItem()
        {
            // Arrange
            var tipo = new TipoRefeicao("Tipo Atualizado");
            var existente = new Item("Item Teste", "Descrição Teste", tipo.Id, new Preco(10.0m));
            var dto = new ItemUpdateDto
            {
                Id = existente.Id,
                Nome = "Item Atualizado",
                Descricao = "Descrição Atualizada",
                TipoRefeicaoId = Guid.NewGuid(),
                Valor = 20.0m
            };
            _mockItemRepository.Setup(repo => repo.ObterPorIdAsync(dto.Id)).ReturnsAsync(existente);
            _mockItemRepository.Setup(repo => repo.Atualizar(existente));
            _mockTipoRefeicaoRepository.Setup(repo => repo.ObterPorIdAsync(dto.TipoRefeicaoId)).ReturnsAsync(tipo);

            // Act
            await _itemService.AtualizarAsync(dto);

            // Assert
            _mockItemRepository.Verify(repo => repo.Atualizar(It.IsAny<Item>()), Times.Once);
            _mockItemRepository.Verify(repo => repo.SalvarAlteracoesAsync(), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_ComItemInexistente_DeveLancarExcecao()
        {
            // Arrange
            var dto = new ItemUpdateDto
            {
                Id = Guid.NewGuid(),
                Nome = "Item Atualizado",
                Descricao = "Descrição Atualizada",
                TipoRefeicaoId = Guid.NewGuid(),
                Valor = 20.0m
            };
            _mockItemRepository.Setup(repo => repo.ObterPorIdAsync(dto.Id)).ReturnsAsync((Item)null!);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _itemService.AtualizarAsync(dto));
            _mockItemRepository.Verify(repo => repo.Atualizar(It.IsAny<Item>()), Times.Never);
        }

        [Fact]
        public async Task AtualizarAsync_ComNomeDuplicado_DeveLancarExcecao()
        {
            // Arrange
            var dto = new ItemUpdateDto
            {
                Id = Guid.NewGuid(),
                Nome = "Item Duplicado",
                Descricao = "Descrição Atualizada",
                TipoRefeicaoId = Guid.NewGuid(),
                Valor = 20.0m
            };

            var tipo = new TipoRefeicao("Tipo Atualizado");
            var existente = new Item("Item Original", "Descrição Original", tipo.Id, new Preco(10.0m));

            _mockItemRepository.Setup(repo => repo.ObterPorIdAsync(dto.Id)).ReturnsAsync(existente);
            _mockItemRepository.Setup(repo => repo.ObterPorNomeAsync(dto.Nome)).ReturnsAsync(existente);
            _mockTipoRefeicaoRepository.Setup(repo => repo.ObterPorIdAsync(dto.TipoRefeicaoId)).ReturnsAsync(tipo);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _itemService.AtualizarAsync(dto));
            _mockItemRepository.Verify(repo => repo.Atualizar(It.IsAny<Item>()), Times.Never);
        }
    }
}
