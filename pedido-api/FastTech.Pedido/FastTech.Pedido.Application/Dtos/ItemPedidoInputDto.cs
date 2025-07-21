using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Dtos
{
    public class ItemPedidoInputDto
    {
        public Guid IdItemCardapio { get; set; }
        public string Nome { get; set; } = null!;
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
    }
}
