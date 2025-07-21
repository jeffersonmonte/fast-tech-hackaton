using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;
using FastTech.Catalogo.Domain.Interfaces.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Application.Services
{
    public class TipoRefeicaoService : ITipoRefeicaoService
    {
        private readonly ITipoRefeicaoQueryRepository _tipoRefeicaoQueryRepository;

        public TipoRefeicaoService(ITipoRefeicaoQueryRepository repository)
        {
            _tipoRefeicaoQueryRepository = repository;
        }

        public async Task<IEnumerable<TipoRefeicaoOutputDto>> ListarTodosAsync()
        {
            var tipos = await _tipoRefeicaoQueryRepository.ListarTodosAsync();
            return tipos is null || !tipos.Any() ? [] : MapearTiposRefeicaoParaOutputs(tipos);
        }

        public async Task<TipoRefeicaoOutputDto?> ObterPorIdAsync(Guid id)
        {
            var tipo = await _tipoRefeicaoQueryRepository.ObterPorIdAsync(id);
            return tipo is null ? null : MapearTipoRefeicaoParaOutput(tipo);
        }

        public async Task<TipoRefeicaoOutputDto?> ObterPorNomeAsync(string nome)
        {
            var tipo = await _tipoRefeicaoQueryRepository.ObterPorNomeAsync(nome);
            return tipo is null ? null : MapearTipoRefeicaoParaOutput(tipo);
        }

        private static IEnumerable<TipoRefeicaoOutputDto> MapearTiposRefeicaoParaOutputs(IEnumerable<TipoRefeicao> itens)
        {
            return itens.Select(i => MapearTipoRefeicaoParaOutput(i));
        }

        private static TipoRefeicaoOutputDto MapearTipoRefeicaoParaOutput(TipoRefeicao item)
        {
            return new TipoRefeicaoOutputDto
            {
                Id = item.Id,
                Nome = item.Nome
            };
        }
    }
}
