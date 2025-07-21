using FastTech.Pedido.Domain.Enums;
using FastTech.Pedido.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Pedido.Domain.Entities
{
    public class Pedido : EntidadeBase
    {
        public string CodigoPedido { get; private set; }
        public Guid IdCliente { get; private set; }
        public ClientePedido Cliente { get; private set; }
        public DateTime DataHoraCriacao { get; private set; }
        public FormaEntrega FormaEntrega { get; private set; }
        public StatusPedido Status { get; private set; }
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
        public string? JustificativaCancelamento { get; private set; }
        public DateTime? DataHoraCancelamento { get; private set; }

        private readonly List<ItemPedido> _itens = [];
        public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();

        private readonly List<StatusPedidoHistorico> _historico = [];
        public IReadOnlyCollection<StatusPedidoHistorico> Historico => _historico.AsReadOnly();

        public Pedido(Guid idCliente, ClientePedido cliente, FormaEntrega formaEntrega)
        {
            Id = Guid.NewGuid();
            CodigoPedido = $"P-{Guid.NewGuid().ToString()[..8].ToUpper()}";
            IdCliente = idCliente;
            Cliente = cliente;
            DataHoraCriacao = DateTime.UtcNow;
            FormaEntrega = formaEntrega;
            Status = StatusPedido.Criado;
            RegistrarStatus(StatusPedido.Criado);
        }

        public void AdicionarItem(Guid idItemCardapio, string nome, decimal precoUnitario, int quantidade)
        {
            if (quantidade <= 0)
                throw new InvalidOperationException("Quantidade deve ser maior que zero.");

            var item = new ItemPedido(idItemCardapio, nome, precoUnitario, quantidade);
            _itens.Add(item);
        }

        public void Cancelar(string justificativa)
        {
            if (Status != StatusPedido.Criado && Status != StatusPedido.AguardandoPreparo)
                throw new InvalidOperationException("Pedido não pode ser cancelado nesse estágio.");

            JustificativaCancelamento = justificativa;
            DataHoraCancelamento = DateTime.UtcNow;
            Status = StatusPedido.Cancelado;
            RegistrarStatus(StatusPedido.Cancelado);
        }

        public void AtualizarStatus(StatusPedido novoStatus, Guid? idFuncionario, string? observacao = null)
        {
            if (Status == StatusPedido.Cancelado || Status == StatusPedido.Entregue)
                throw new InvalidOperationException("Não é possível atualizar um pedido finalizado.");

            Status = novoStatus;
            RegistrarStatus(novoStatus, idFuncionario, observacao);
        }

        private void RegistrarStatus(StatusPedido status, Guid? idFuncionario = null, string? observacao = null)
        {
            _historico.Add(new StatusPedidoHistorico(status, idFuncionario, observacao));
        }
    }
}
