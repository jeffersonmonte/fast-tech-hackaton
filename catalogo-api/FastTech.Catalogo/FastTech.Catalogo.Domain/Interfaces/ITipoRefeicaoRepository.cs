using FastTech.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Domain.Interfaces
{
    public interface ITipoRefeicaoRepository : IRepositoryBase<TipoRefeicao>
    {
        Task<TipoRefeicao?> ObterPorIdAsync(Guid id);
        Task<TipoRefeicao?> ObterPorNomeAsync(string nome);
    }
}
