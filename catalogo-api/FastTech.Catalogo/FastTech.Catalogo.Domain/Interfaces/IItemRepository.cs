using FastTech.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Domain.Interfaces
{
    public interface IItemRepository : IRepositoryBase<Item>
    {
        Task<Item?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Item>> ListarPorTipoAsync(Guid tipoRefeicaoId);
        Task<Item?> ObterPorNomeAsync(string nome);
    }
}
