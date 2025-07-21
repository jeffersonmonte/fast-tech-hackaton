namespace FastTech.Kitchen.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public List<PedidoItem> Itens { get; private set; }
        public string Status { get; private set; }

        public Pedido(Guid id, List<PedidoItem> itens)
        {
            Id = id;
            Itens = itens;
            Status = "Recebido";
        }

        public void Aceitar()
        {
            Status = "Em Preparo";
        }

        public void Recusar(string motivo)
        {
            Status = $"Recusado: {motivo}";
        }
    }

    public class PedidoItem
    {
        public string NomeProduto { get; private set; }
        public int Quantidade { get; private set; }

        public PedidoItem(string nomeProduto, int quantidade)
        {
            NomeProduto = nomeProduto;
            Quantidade = quantidade;
        }
    }
}
