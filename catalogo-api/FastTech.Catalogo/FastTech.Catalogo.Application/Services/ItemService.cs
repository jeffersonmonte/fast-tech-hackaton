using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;
using FastTech.Catalogo.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemCommandRepository _itemCommandRepository;
        private readonly IItemQueryRepository _itemQueryRepository;
        private readonly ITipoRefeicaoQueryRepository _tipoRefeicaoQueryRepository;
        private readonly IEventPublisher _eventPublisher;

        public ItemService(
            IItemCommandRepository itemCommandRepository
            , IItemQueryRepository itemQueryRepository
            , ITipoRefeicaoQueryRepository tipoRefeicaoQueryRepository
            , IEventPublisher eventPublisher
            )
        {
            _itemCommandRepository = itemCommandRepository;
            _itemQueryRepository = itemQueryRepository;
            _tipoRefeicaoQueryRepository = tipoRefeicaoQueryRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<ItemOutputDto?> ObterPorIdAsync(Guid id)
        {
            var item = await _itemQueryRepository.ObterPorIdAsync(id);
            return item is null ? null : MapearItemParaOutput(item);
        }

        public async Task<IEnumerable<ItemOutputDto>> ListarTodosAsync()
        {
            var itens = await _itemQueryRepository.ListarTodosAsync();
            return MapearItensParaOutputs(itens);
        }

        public async Task<IEnumerable<ItemOutputDto>> ListarPorTipoAsync(Guid tipoRefeicaoId)
        {
            var itens = await _itemQueryRepository.ListarPorTipoAsync(tipoRefeicaoId);
            return itens is null || !itens.Any() ? [] : MapearItensParaOutputs(itens);
        }

        public async Task<Guid> AdicionarAsync(ItemInputDto dto)
        {
            var tipo = await _tipoRefeicaoQueryRepository.ObterPorIdAsync(dto.TipoRefeicaoId);
            if (tipo is null)
                throw new InvalidOperationException("Tipo de refeição inválido.");

            var itemMesmoNome = await _itemQueryRepository.ObterPorNomeAsync(dto.Nome);
            if (itemMesmoNome is not null)
                throw new InvalidOperationException("Já existe um item com o mesmo nome.");

            var item = new Item(dto.Nome, dto.Descricao, tipo.Id, new Preco(dto.Valor));
            await _itemCommandRepository.AdicionarAsync(item);
            await _itemCommandRepository.SalvarAlteracoesAsync();

            await _eventPublisher.PublishAsync("catalogo.item", "item.created", new ItemCreatedEvent
            {
                Id = item.Id,
                Nome = item.Nome,
                Descricao = item.Descricao,
                TipoRefeicaoId = item.TipoRefeicaoId,
                Valor = item.Preco.Valor
            });

            return item.Id;
        }

        public async Task AtualizarAsync(ItemUpdateDto dto)
        {
            var existente = await _itemCommandRepository.ObterPorIdAsync(dto.Id);
            if (existente is null)
                throw new ArgumentException("Item não encontrado.");

            var tipo = await _tipoRefeicaoQueryRepository.ObterPorIdAsync(dto.TipoRefeicaoId);
            if (tipo is null)
                throw new ArgumentException("Tipo de refeição inválido.");

            var itemMesmoNome = await _itemQueryRepository.ObterPorNomeAsync(dto.Nome);
            if (itemMesmoNome is not null && itemMesmoNome.Id != dto.Id)
                throw new InvalidOperationException("Já existe um item com o mesmo nome.");

            existente.Atualizar(dto.Nome, dto.Descricao, tipo.Id, new Preco(dto.Valor));

            await _itemCommandRepository.SalvarAlteracoesAsync();

            await _eventPublisher.PublishAsync("catalogo.item", "item.updated", new ItemUpdatedEvent
            {
                Id = existente.Id,
                Nome = existente.Nome,
                Descricao = existente.Descricao,
                TipoRefeicaoId = existente.TipoRefeicaoId,
                Valor = existente.Preco.Valor
            });

            await _itemCommandRepository.SalvarAlteracoesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var existente = await _itemCommandRepository.ObterPorIdAsync(id);
            if (existente is null)
                throw new InvalidOperationException("Item não encontrado.");

            existente.Excluir();

            await _itemCommandRepository.SalvarAlteracoesAsync();
        }

        private static IEnumerable<ItemOutputDto> MapearItensParaOutputs(IEnumerable<Item> itens)
        {
            return itens.Select(i => MapearItemParaOutput(i));
        }

        private static ItemOutputDto MapearItemParaOutput(Item item)
        {
            return new ItemOutputDto
            {
                Id = item.Id,
                Nome = item.Nome,
                Descricao = item.Descricao,
                TipoRefeicaoNome = item.TipoRefeicao?.Nome ?? string.Empty,
                TipoRefeicaoId= item.TipoRefeicaoId,
                Valor = item.Preco.Valor
            };
        }
    }
}
