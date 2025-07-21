using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Application.Interfaces
{
    public interface ITipoRefeicaoService
    {
        Task<IEnumerable<TipoRefeicaoOutputDto>> ListarTodosAsync();
        Task<TipoRefeicaoOutputDto?> ObterPorIdAsync(Guid id);
        Task<TipoRefeicaoOutputDto?> ObterPorNomeAsync(string nome);
    }
}
