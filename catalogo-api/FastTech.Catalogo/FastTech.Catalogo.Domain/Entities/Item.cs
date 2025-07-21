using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastTech.Catalogo.Domain.ValueObjects;

namespace FastTech.Catalogo.Domain.Entities
{
    public class Item : EntidadeBase
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public TipoRefeicao TipoRefeicao { get; private set; }
        public Guid TipoRefeicaoId { get; private set; }
        public Preco Preco { get; private set; }
        public ICollection<Cardapio> Cardapios { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataEdicao { get; private set; }
        public DateTime? DataExclusao { get; private set; }

        public Item() { }
        public Item(string nome, string descricao, Guid idTipo, Preco preco)
        {
            ValidarDados(nome, descricao, idTipo, preco);

            Id = Guid.NewGuid();
            Nome = nome;
            Descricao = descricao;
            TipoRefeicao = null!;
            TipoRefeicaoId = idTipo;
            Preco = preco;
            Cardapios = [];
            DataCriacao = DateTime.UtcNow;
            DataEdicao = null;
            DataExclusao = null;
        }

        public void Atualizar(string nome, string descricao, Guid idTipo, Preco preco)
        {
            ValidarDados(nome, descricao, idTipo, preco);

            Nome = nome;
            Descricao = descricao;
            TipoRefeicao = null!;
            TipoRefeicaoId = idTipo;
            Preco = preco;
            DataEdicao = DateTime.UtcNow;
        }

        public void Excluir()
        {
            if (DataExclusao != null)
                throw new InvalidOperationException("O item já foi excluído.");

            DataExclusao = DateTime.UtcNow;
        }

        public void AssociarCardapio(Cardapio cardapio)
        {
            if (cardapio == null)
                throw new ArgumentException("Cardápio não pode ser nulo.");

            if (!Cardapios.Contains(cardapio))
                Cardapios.Add(cardapio);
        }

        public void RemoverCardapio(Cardapio cardapio)
        {
            if (cardapio == null || !Cardapios.Contains(cardapio))
                throw new ArgumentException("Cardápio inválido para remoção.");

            Cardapios.Remove(cardapio);
        }

        private static void ValidarDados(string nome, string descricao, Guid idTipo, Preco preco)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (nome.Length > 250)
                throw new ArgumentException("Nome deve ter no máximo 250 caracteres.");

            if (descricao.Length > 500)
                throw new ArgumentException("Descrição deve ter no máximo 500 caracteres.");

            if (idTipo == Guid.Empty)
                throw new ArgumentNullException(nameof(idTipo), "Tipo de refeição é obrigatório.");

            if (preco is null)
                throw new ArgumentNullException(nameof(preco), "Preço é obrigatório.");
        }
    }
}
