using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Dtos
{
    public class ClientePedidoDto
    {
        public Guid IdCliente { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
