using System;
using System.Collections.Generic;

namespace FastTech.Catalogo.Domain.Entities
{
    public class Cardapio : EntidadeBase
    {
        public string Nome { get; private set; }
        public string? Descricao { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataEdicao { get; private set; }
        public DateTime? DataExclusao { get; private set; }

        public ICollection<Item> Itens { get; private set; } = [];

        private readonly List<Guid> _itensPendentes = [];
        public IReadOnlyCollection<Guid> ItensPendentes => _itensPendentes.AsReadOnly();

        protected Cardapio() { }

        public Cardapio(string nome, string? descricao, DateTime dataCriacao)
        {
            Id = Guid.NewGuid();
            ValidarDados(nome, descricao);

            Nome = nome;
            Descricao = descricao;
            DataCriacao = dataCriacao;
        }

        public void Atualizar(string nome, string? descricao)
        {
            ValidarDados(nome, descricao);
            Nome = nome;
            Descricao = descricao;
            DataEdicao = DateTime.UtcNow;
        }

        public void Excluir()
        {
            if (DataExclusao != null)
                throw new InvalidOperationException("O cardápio já foi excluído.");

            DataExclusao = DateTime.UtcNow;
        }

        public void AssociarIdItens(IEnumerable<Guid> itensId)
        {
            if (itensId is null || !itensId.Any())
                throw new ArgumentException("Necessário preencher ao menos um item.");

            foreach (var item in itensId)
                AssociarIdItem(item);
        }

        public void AssociarIdItem(Guid itemId)
        {
            if (itemId == Guid.Empty)
                throw new ArgumentException("Id do item inválido.");

            if (!_itensPendentes.Contains(itemId))
                _itensPendentes.Add(itemId);
        }

        public void LimparIdItensAssociados()
        {
            _itensPendentes.Clear();
        }

        public void AdicionarItem(Item item)
        {
            if (item == null)
                throw new ArgumentException("Item não pode ser nulo.");

            if (!Itens.Any(i => i.Id == item.Id))
                Itens.Add(item);
        }

        public void RemoverItem(Item item)
        {
            if (item == null || !Itens.Contains(item))
                throw new ArgumentException("Item inválido para remoção.");

            Itens.Remove(item);
        }

        public IEnumerable<Item> ListarItens()
        {
            return Itens;
        }

        private static void ValidarDados(string nome, string? descricao)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (nome.Length > 100)
                throw new ArgumentException("Nome deve ter no máximo 100 caracteres.");

            if (descricao != null && descricao.Length > 500)
                throw new ArgumentException("Descrição deve ter no máximo 500 caracteres.");
        }
    }
}
