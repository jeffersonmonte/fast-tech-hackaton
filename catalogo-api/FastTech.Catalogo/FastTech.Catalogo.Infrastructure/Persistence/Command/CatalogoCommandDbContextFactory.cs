using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace FastTech.Catalogo.Infrastructure.Persistence.Command
{
    public class CatalogoCommandDbContextFactory : IDesignTimeDbContextFactory<CatalogoCommandDbContext>
    {
        public CatalogoCommandDbContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new InvalidOperationException("Nenhuma string de conexão encontrada no serviço de cardapio.");

            var optionsBuilder = new DbContextOptionsBuilder<CatalogoCommandDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new CatalogoCommandDbContext(optionsBuilder.Options);
        }
    }
}
