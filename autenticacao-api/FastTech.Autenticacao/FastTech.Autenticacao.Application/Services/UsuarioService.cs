using FastTech.Autenticacao.Application.Dtos;
using FastTech.Autenticacao.Application.Interfaces;
using FastTech.Autenticacao.Domain.Entities;
using FastTech.Autenticacao.Domain.Interfaces;
using FastTech.Autenticacao.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<Guid> CadastrarAsync(UsuarioCadastroDto dto)
        {
            if (await _usuarioRepository.ExisteComEmailAsync(dto.Email))
                throw new InvalidOperationException("E-mail já cadastrado.");

            if (!string.IsNullOrWhiteSpace(dto.Cpf) && await _usuarioRepository.ExisteComCpfAsync(dto.Cpf))
                throw new InvalidOperationException("CPF já cadastrado.");

            var email = new Email(dto.Email);
            var senha = new Senha(dto.Senha);
            var cpf = string.IsNullOrWhiteSpace(dto.Cpf) ? null : new Cpf(dto.Cpf);

            var usuario = new Usuario(dto.Nome, email, senha, dto.Perfil, cpf);

            await _usuarioRepository.AdicionarAsync(usuario);
            await _usuarioRepository.SalvarAlteracoesAsync();

            return usuario.Id;
        }

        public async Task<UsuarioOutputDto?> AutenticarAsync(UsuarioLoginDto dto)
        {
            Usuario? usuario = null;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);

            else if (!string.IsNullOrWhiteSpace(dto.Cpf))
                usuario = await _usuarioRepository.ObterPorCpfAsync(dto.Cpf);

            if (usuario == null || !usuario.VerificarSenha(dto.Senha))
                return null;

            var usuarioRetornoDto = new UsuarioOutputDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email.Endereco,
                Perfil = usuario.Perfil.ToString()
            };

            string token = _tokenService.GerarToken(usuarioRetornoDto);
            usuarioRetornoDto.Token = token;

            return usuarioRetornoDto;
        }

        public async Task<UsuarioOutputDto?> ObterPorIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario is null)
                return null;

            var usuarioRetornoDto = new UsuarioOutputDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email.Endereco,
                Perfil = usuario.Perfil.ToString()
            };

            return usuarioRetornoDto;
        }

        public async Task AtualizarSenhaAsync(Guid idUsuario, string novaSenha)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(idUsuario)
                          ?? throw new InvalidOperationException("Usuário não encontrado.");

            usuario.AtualizarSenha(new Senha(novaSenha));
            _usuarioRepository.Atualizar(usuario);
            await _usuarioRepository.SalvarAlteracoesAsync();
        }

        public async Task InativarAsync(Guid idUsuario)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(idUsuario)
                          ?? throw new InvalidOperationException("Usuário não encontrado.");

            usuario.Inativar();
            _usuarioRepository.Atualizar(usuario);
            await _usuarioRepository.SalvarAlteracoesAsync();
        }

        public async Task ReativarAsync(Guid idUsuario)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(idUsuario)
                          ?? throw new InvalidOperationException("Usuário não encontrado.");

            usuario.Reativar();
            _usuarioRepository.Atualizar(usuario);
            await _usuarioRepository.SalvarAlteracoesAsync();
        }
    }
}