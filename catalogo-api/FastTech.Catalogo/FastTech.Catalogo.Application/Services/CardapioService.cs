using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;

namespace FastTech.Catalogo.Application.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly ICardapioRepository _cardapioRepository;
        private readonly IItemRepository _itemRepository;

        public CardapioService(ICardapioRepository cardapioRepository, IItemRepository itemRepository)
        {
            _cardapioRepository = cardapioRepository;
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<CardapioOutputDto>> ObterTodosAsync()
        {
            var cardapios = await _cardapioRepository.ListarTodosAsync();
            return cardapios is null || !cardapios.Any() ? [] : MapearCardapiosParaOutputs(cardapios);
        }

        public async Task<CardapioOutputDto?> ObterPorIdAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException("O ID do cardápio não pode ser vazio.", nameof(id));

            var cardapio = await _cardapioRepository.ObterPorIdAsync(id);
            return cardapio is null ? null : MapearCardapioParaOutput(cardapio);
        }

        public async Task<Guid> AdicionarAsync(CardapioInputDto cardapio)
        {
            var entidade = new Cardapio(cardapio.Nome, cardapio.Descricao, DateTime.UtcNow);

            var itens = await _itemRepository.ListarAsync(i => cardapio.ItensIds.Contains(i.Id));

            if (itens.Count() != cardapio.ItensIds.Count())
                throw new ArgumentException("Alguns itens não existem ou não estão disponíveis.");

            entidade.AssociarIdItens(itens.Select(i => i.Id));

            if(await _cardapioRepository.ExisteAsync(c => c.Nome == cardapio.Nome && c.DataExclusao == null))
                throw new InvalidOperationException("Já existe um cardápio com esse nome.");

            await _cardapioRepository.AdicionarAsync(entidade);
            await _cardapioRepository.SalvarAlteracoesAsync();

            return entidade.Id;
        }

        public async Task AtualizarAsync(CardapioUpdateDto cardapio)
        {
            var entidade = await _cardapioRepository.ObterPorIdAsync(cardapio.Id);

            if(entidade is null)
                throw new ArgumentException("Cardápio não encontrado.");

            var entidadeMesmoNome = await _cardapioRepository.ObterPorNomeAsync(cardapio.Nome);

            if(entidadeMesmoNome is not null)
                throw new InvalidOperationException("Já existe um cardápio com esse nome.");

            var itens = await _itemRepository.ListarAsync(i => cardapio.ItensIds.Contains(i.Id));

            if (itens.Count() != cardapio.ItensIds.Count())
                throw new ArgumentException("Alguns itens não existem ou não estão disponíveis.");

            entidade.Atualizar(cardapio.Nome, cardapio.Descricao);
            entidade.AssociarIdItens(itens.Select(i => i.Id));

            _cardapioRepository.Atualizar(entidade);
            await _cardapioRepository.SalvarAlteracoesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var entidade = await _cardapioRepository.ObterPorIdAsync(id);

            if (entidade is null)
                throw new InvalidOperationException("Cardápio não encontrado.");

            entidade.Excluir();

            _cardapioRepository.Remover(entidade);
            await _cardapioRepository.SalvarAlteracoesAsync();
        }

        private static IEnumerable<CardapioOutputDto> MapearCardapiosParaOutputs(IEnumerable<Cardapio> cardapios)
        {
            return cardapios.Select(c => MapearCardapioParaOutput(c));
        }

        private static CardapioOutputDto MapearCardapioParaOutput(Cardapio cardapio)
        {
            return new CardapioOutputDto
            {
                Id = cardapio.Id,
                Nome = cardapio.Nome,
                Descricao = cardapio.Descricao,
                Itens = MapearItensParaOutputs(cardapio.Itens)
            };
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
                TipoRefeicaoId = item.TipoRefeicaoId,
                Valor = item.Preco.Valor
            };
        }
    }
}
