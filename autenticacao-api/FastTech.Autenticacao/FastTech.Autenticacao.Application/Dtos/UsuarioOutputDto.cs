using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Application.Dtos
{
    public class UsuarioOutputDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Perfil { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
