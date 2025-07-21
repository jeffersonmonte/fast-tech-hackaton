using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Application.Dtos
{
    public class StatusPedidoHistoricoOutputDto
    {
        public string Status { get; set; } = null!;
        public DateTime DataHora { get; set; }
        public Guid? IdFuncionarioResponsavel { get; set; }
        public string? Observacao { get; set; }
    }
}
