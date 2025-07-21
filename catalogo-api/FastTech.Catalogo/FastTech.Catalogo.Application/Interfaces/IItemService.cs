using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Application.Interfaces
{
    public interface IItemService
    {
        Task<ItemOutputDto?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ItemOutputDto>> ListarTodosAsync();
        Task<IEnumerable<ItemOutputDto>> ListarPorTipoAsync(Guid tipoRefeicaoId);
        Task<Guid> AdicionarAsync(ItemInputDto dto);
        Task AtualizarAsync(ItemUpdateDto dto);
        Task RemoverAsync(Guid id);
    }
}
