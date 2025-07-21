namespace FastTech.Catalogo.Domain.ValueObjects
{
    public class Preco
    {
        public decimal Valor { get; private set; }

        protected Preco() { }

        public Preco(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Preço deve ser maior que zero.");

            Valor = valor;
        }
    }
}
