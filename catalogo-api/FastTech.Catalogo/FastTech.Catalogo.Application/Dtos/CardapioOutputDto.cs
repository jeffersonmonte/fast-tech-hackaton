using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Application.Dtos
{
    public class CardapioOutputDto
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public IEnumerable<ItemOutputDto>? Itens { get; set; }
    }
}
