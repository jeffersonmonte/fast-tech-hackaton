using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Application.Dtos
{
    public class CardapioUpdateDto
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required IEnumerable<Guid> ItensIds { get; set; }
    }
}
