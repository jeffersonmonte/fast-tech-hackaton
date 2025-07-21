using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Enums
{
    public enum StatusPedido
    {
        Criado,
        AguardandoPreparo,
        EmPreparo,
        Pronto,
        Entregue,
        Recusado,
        Cancelado
    }

    public enum FormaEntrega
    {
        Balcao,
        DriveThru,
        Delivery
    }
}
