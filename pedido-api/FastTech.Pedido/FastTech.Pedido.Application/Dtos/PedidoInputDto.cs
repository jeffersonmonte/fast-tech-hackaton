using FastTech.Pedido.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Dtos
{
    public class PedidoInputDto
    {
        public ClientePedidoDto Cliente { get; set; } = null!;
        public FormaEntrega FormaEntrega { get; set; }
        public List<ItemPedidoInputDto> Itens { get; set; } = [];
    }
}
