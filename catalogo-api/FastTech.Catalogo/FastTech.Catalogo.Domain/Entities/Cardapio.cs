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

        private readonly List<Item> _itens = [];
        public IReadOnlyCollection<Item> Itens => _itens.AsReadOnly();

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

        public void AdicionarItens(IEnumerable<Item> itens)
        {
            if (itens == null || !itens.Any())
                throw new ArgumentException("Item não pode ser nulo.");

            foreach (var item in itens)
                AdicionarItem(item);
        }

        public void AdicionarItem(Item item)
        {
            if (item == null)
                throw new ArgumentException("Item não pode ser nulo.");

            if (!Itens.Any(i => i.Id == item.Id))
                _itens.Add(item);
        }

        public void RemoverItem(Item item)
        {
            if (item == null || !Itens.Contains(item))
                throw new ArgumentException("Item inválido para remoção.");

            _itens.Remove(item);
        }

        public void LimparItens()
        {
            _itens.Clear();
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
