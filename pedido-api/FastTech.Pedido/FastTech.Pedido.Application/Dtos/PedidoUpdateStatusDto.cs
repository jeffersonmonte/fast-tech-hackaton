using FastTech.Pedido.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Dtos
{
    public class PedidoUpdateStatusDto
    {
        public Guid IdPedido { get; set; }
        public StatusPedido NovoStatus { get; set; }
        public Guid? IdFuncionario { get; set; }
        public string? Observacao { get; set; }
    }
}
