using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.ValueObjects
{
    public class ClientePedido
    {
        public Guid IdCliente { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public ClientePedido(Guid idCliente, string nome, string email)
        {
            IdCliente = idCliente;
            Nome = nome;
            Email = email;
        }
    }
}
