namespace FastTech.Catalogo.Application.Dtos
{
    public class CardapioInputDto
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required IEnumerable<Guid> ItensIds { get; set; }
    }
}
