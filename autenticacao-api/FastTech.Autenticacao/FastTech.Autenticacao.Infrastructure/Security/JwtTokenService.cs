using FastTech.Autenticacao.Application.Dtos;
using FastTech.Autenticacao.Application.Interfaces;
using FastTech.Autenticacao.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FastTech.Autenticacao.Infrastructure.Security
{
    internal class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(UsuarioOutputDto usuario)
        {
            var chaveSecreta = _configuration["SecretJWT"]!;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenPropriedades = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("Nome", usuario.Nome),
                    new Claim("Email", usuario.Email),
                    new Claim("Perfil", usuario.Perfil),
                    new Claim("Id", usuario.Id.ToString())
                ]),

                Expires = DateTime.UtcNow.AddHours(8),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenPropriedades);
            var tokenPlainText = tokenHandler.WriteToken(token);
            return tokenPlainText;
        }
    }
}
