using FastTech.Autenticacao.Domain.Enums;
using FastTech.Autenticacao.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Domain.Entities
{
    public class Usuario : EntidadeBase
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public Senha Senha { get; private set; }
        public Cpf? Cpf { get; private set; }
        public PerfilUsuario Perfil { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Usuario() { }

        public Usuario(string nome, Email email, Senha senha, PerfilUsuario perfil, Cpf? cpf = null)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Senha = senha;
            Perfil = perfil;
            Cpf = cpf;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
        }

        public void AtualizarSenha(Senha novaSenha)
        {
            Senha = novaSenha;
        }

        public void Inativar() => Ativo = false;

        public void Reativar() => Ativo = true;

        public bool VerificarSenha(string senhaEmTextoClaro)
        {
            return Senha.Verificar(senhaEmTextoClaro);
        }
    }
}
