using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Entities
{
    public class ItemPedido : EntidadeBase
    {
        public Guid IdItemCardapio { get; private set; }
        public string Nome { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Subtotal => PrecoUnitario * Quantidade;

        public ItemPedido(Guid idItemCardapio, string nome, decimal precoUnitario, int quantidade)
        {
            Id = Guid.NewGuid();
            IdItemCardapio = idItemCardapio;
            Nome = nome;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
        }
    }
}
