namespace FastTech.Catalogo.Application.Dtos
{
    public class ItemCreatedEvent
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public Guid TipoRefeicaoId { get; set; }
        public decimal Valor { get; set; }
    }
}
