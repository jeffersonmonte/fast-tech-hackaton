using FastTech.Autenticacao.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Application.Interfaces
{
    public interface ITokenService
    {
        public string GerarToken(UsuarioOutputDto usuarioDto);
    }
}
