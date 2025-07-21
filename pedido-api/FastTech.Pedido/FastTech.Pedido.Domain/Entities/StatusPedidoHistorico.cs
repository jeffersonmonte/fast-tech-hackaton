using FastTech.Pedido.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Entities
{
    public class StatusPedidoHistorico : EntidadeBase
    {
        public Guid PedidoId { get; set; }
        public StatusPedido Status { get; private set; }
        public DateTime DataHora { get; private set; }
        public Guid? IdFuncionarioResponsavel { get; private set; }
        public string? Observacao { get; private set; }

        public StatusPedidoHistorico() { }
        public StatusPedidoHistorico(Guid pedidoId, StatusPedido status, Guid? idFuncionario, string? observacao)
        {
            Id = Guid.NewGuid();
            Status = status;
            DataHora = DateTime.UtcNow;
            IdFuncionarioResponsavel = idFuncionario;
            Observacao = observacao;
            PedidoId = pedidoId;
        }
    }
}
