using FastTech.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Domain.Interfaces.Query
{
    public interface IItemQueryRepository : IQueryRepository<Item>
    {
        Task<Item?> ObterPorNomeAsync(string nome);
        Task<IEnumerable<Item>> ListarPorTipoAsync(Guid idTipoRefeicao);
    }
}
