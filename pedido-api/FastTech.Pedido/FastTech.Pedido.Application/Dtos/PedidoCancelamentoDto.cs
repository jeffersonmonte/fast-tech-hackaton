using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Dtos
{
    public class PedidoCancelamentoDto
    {
        public Guid IdPedido { get; set; }
        public string Justificativa { get; set; } = null!;
    }
}
