using FastTech.Autenticacao.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Guid> CadastrarAsync(UsuarioCadastroDto dto);
        Task<UsuarioOutputDto?> AutenticarAsync(UsuarioLoginDto dto);
        Task<UsuarioOutputDto?> ObterPorIdAsync(Guid id);
        Task AtualizarSenhaAsync(Guid idUsuario, string novaSenha);
        Task InativarAsync(Guid idUsuario);
        Task ReativarAsync(Guid idUsuario);
    }
}
