using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Domain.Entities
{
    public class TipoRefeicao : EntidadeBase
    {
        public string Nome { get; private set; }
        public DateTime? DataEdicao { get; private set; }
        public DateTime? DataExclusao { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public IReadOnlyCollection<Item> Itens => _itens.AsReadOnly();
        private readonly List<Item> _itens = [];

        public TipoRefeicao() { }
        public TipoRefeicao(string nome)
        {
            Validar(nome);
            Nome = nome;
            DataCriacao = DateTime.UtcNow;
            DataEdicao = null;
            DataExclusao = null;
        }

        public void AtualizarNome(string nome)
        {
            Validar(nome);
            Nome = nome;
        }

        public void Atualizar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            Nome = nome;
            DataEdicao = DateTime.UtcNow;
        }

        public void Excluir()
        {
            if (DataExclusao != null)
                throw new InvalidOperationException("O tipo de refeição já foi excluído.");

            DataExclusao = DateTime.UtcNow;
        }

        private static void Validar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (nome.Length > 100)
                throw new ArgumentException("Nome deve ter no máximo 100 caracteres.");
        }
    }
}
