using System.Text;
using System.Security.Cryptography;

namespace FastTech.Autenticacao.Domain.ValueObjects
{
    public class Senha
    {
        public string Hash { get; private set; }

        public Senha() { }

        public Senha(string senhaTextoPlano)
        {
            if (string.IsNullOrWhiteSpace(senhaTextoPlano) || senhaTextoPlano.Length < 6)
                throw new ArgumentException("Senha inválida");

            Hash = GerarHash(senhaTextoPlano);
        }

        public bool Verificar(string senhaTextoPlano)
        {
            return Hash == GerarHash(senhaTextoPlano);
        }

        private string GerarHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        public override bool Equals(object? obj) =>
            obj is Senha senha && Hash == senha.Hash;

        public override int GetHashCode() => Hash.GetHashCode();
    }
}
