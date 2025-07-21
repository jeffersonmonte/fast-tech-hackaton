using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Domain.Interfaces.Query;

namespace FastTech.Catalogo.Application.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly ICardapioCommandRepository _cardapioCommandRepository;
        private readonly ICardapioQueryRepository _cardapioQueryRepository;
        private readonly IItemCommandRepository _itemCommandRepository;
        private readonly IItemQueryRepository _itemQueryRepository;

        public CardapioService(
            ICardapioCommandRepository cardapioCommandRepository
            , ICardapioQueryRepository cardapioQueryRepository
            , IItemCommandRepository itemCommandRepository
            , IItemQueryRepository itemQueryRepository
            )
        {
            _cardapioCommandRepository = cardapioCommandRepository;
            _cardapioQueryRepository = cardapioQueryRepository;
            _itemCommandRepository = itemCommandRepository;
            _itemQueryRepository = itemQueryRepository;
        }

        public async Task<IEnumerable<CardapioOutputDto>> ObterTodosAsync()
        {
            var cardapios = await _cardapioQueryRepository.ListarTodosAsync();
            return cardapios is null || !cardapios.Any() ? [] : MapearCardapiosParaOutputs(cardapios);
        }

        public async Task<CardapioOutputDto?> ObterPorIdAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException("O ID do cardápio não pode ser vazio.", nameof(id));

            var cardapio = await _cardapioQueryRepository.ObterPorIdAsync(id);
            return cardapio is null ? null : MapearCardapioParaOutput(cardapio);
        }

        public async Task<Guid> AdicionarAsync(CardapioInputDto cardapio)
        {
            var entidade = new Cardapio(cardapio.Nome, cardapio.Descricao, DateTime.UtcNow);

            var itens = await _itemCommandRepository.ListarAsync(i => cardapio.ItensIds.Contains(i.Id) && i.DataExclusao == null);

            if (itens.Count() != cardapio.ItensIds.Count())
                throw new ArgumentException("Alguns itens não existem ou não estão disponíveis.");

            entidade.AdicionarItens(itens);

            if(await _cardapioQueryRepository.ExisteAsync(c => c.Nome == cardapio.Nome && c.DataExclusao == null))
                throw new InvalidOperationException("Já existe um cardápio com esse nome.");

            await _cardapioCommandRepository.AdicionarAsync(entidade);
            await _cardapioCommandRepository.SalvarAlteracoesAsync();

            return entidade.Id;
        }

        public async Task AtualizarAsync(CardapioUpdateDto dto)
        {
            var entidade = await _cardapioCommandRepository.ObterPorIdAsync(dto.Id);
            if (entidade is null)
                throw new ArgumentException("Cardápio não encontrado.");

            var existeMesmoNome = await _cardapioQueryRepository.ExisteAsync(c =>
                                            c.Nome.ToLower().Equals(dto.Nome.ToLower()) 
                                            && c.Id != dto.Id 
                                            && c.DataExclusao == null);
            if (existeMesmoNome)
                throw new InvalidOperationException("Já existe um cardápio com esse nome.");

            var itens = await _itemCommandRepository.ListarAsync(i => dto.ItensIds.Contains(i.Id) && i.DataExclusao == null);
            if (itens.Count() != dto.ItensIds.Count())
                throw new ArgumentException("Alguns itens não existem ou foram excluídos.");

            entidade.Atualizar(dto.Nome, dto.Descricao);

            await _cardapioCommandRepository.LimparItensESalvarAsync(entidade.Id);

            entidade.LimparItens();
            entidade.AdicionarItens(itens);

            await _cardapioCommandRepository.SalvarAlteracoesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var entidade = await _cardapioCommandRepository.ObterPorIdAsync(id);
            if (entidade is null)
                throw new InvalidOperationException("Cardápio não encontrado.");

            entidade.Excluir();

            await _cardapioCommandRepository.SalvarAlteracoesAsync();
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
