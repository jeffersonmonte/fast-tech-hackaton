﻿using FastTech.Catalogo.Application.Dtos;
using FastTech.Catalogo.Application.Interfaces;
using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces;

namespace FastTech.Catalogo.Application.Services
{
    public class TipoRefeicaoService : ITipoRefeicaoService
    {
        private readonly ITipoRefeicaoRepository _repository;

        public TipoRefeicaoService(ITipoRefeicaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TipoRefeicaoOutputDto>> ListarTodosAsync()
        {
            var tipos = await _repository.ListarTodosAsync();
            return tipos is null || !tipos.Any() ? [] : MapearTiposRefeicaoParaOutputs(tipos);
        }

        public async Task<TipoRefeicaoOutputDto?> ObterPorIdAsync(Guid id)
        {
            var tipo = await _repository.ObterPorIdAsync(id);
            return tipo is null ? null : MapearTipoRefeicaoParaOutput(tipo);
        }

        public async Task<TipoRefeicaoOutputDto?> ObterPorNomeAsync(string nome)
        {
            var tipo = await _repository.ObterPorNomeAsync(nome);
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
