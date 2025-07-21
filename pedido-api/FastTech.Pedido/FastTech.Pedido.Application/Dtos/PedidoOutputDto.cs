using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Dtos
{
    public class PedidoOutputDto
    {
        public Guid Id { get; set; }
        public string CodigoPedido { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string FormaEntrega { get; set; } = null!;
        public decimal ValorTotal { get; set; }
        public DateTime DataHoraCriacao { get; set; }

        public List<ItemPedidoOutputDto> Itens { get; set; } = [];
        public List<StatusPedidoHistoricoOutputDto> Historico { get; set; } = [];
    }
}
