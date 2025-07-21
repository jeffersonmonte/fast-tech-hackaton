using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Application.Dtos
{
    public class UsuarioLoginDto
    {
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public string Senha { get; set; } = null!;
    }
}
