using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Domain.ValueObjects
{
    public class Email
    {
        public string Endereco { get; private set; }

        public Email() { }

        public Email(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco) || !endereco.Contains('@'))
                throw new ArgumentException("Email inválido");

            Endereco = endereco.Trim().ToLower();
        }

        public override string ToString() => Endereco;

        public override bool Equals(object? obj) =>
            obj is Email email && Endereco == email.Endereco;

        public override int GetHashCode() => Endereco.GetHashCode();
    }
}
