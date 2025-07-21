using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Domain.ValueObjects
{
    public class Cpf
    {
        public string Numero { get; private set; }

        public Cpf() { }

        public Cpf(string numero)
        {
            if (!IsCpfValido(numero))
                throw new ArgumentException("CPF inválido");

            Numero = numero.Trim().Replace(".", "").Replace("-", "");
        }

        private static bool IsCpfValido(string cpf)
        {
            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            return cpf.Length == 11 && cpf.All(char.IsDigit);
        }

        public override string ToString() => Numero;

        public override bool Equals(object? obj) =>
            obj is Cpf cpf && Numero == cpf.Numero;

        public override int GetHashCode() => Numero.GetHashCode();
    }
}
