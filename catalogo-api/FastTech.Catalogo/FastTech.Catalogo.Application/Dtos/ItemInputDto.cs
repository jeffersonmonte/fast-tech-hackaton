namespace FastTech.Catalogo.Application.Dtos
{
    public class ItemInputDto
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required Guid TipoRefeicaoId { get; set; }
        public required decimal Valor { get; set; }
    }
}
