using FastTech.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Domain.Interfaces.Command
{
    public interface ICardapioCommandRepository : ICommandRepository<Cardapio>
    {
        Task LimparItensESalvarAsync(Guid cardapioId);
    }
}
