using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FastTech.Kitchen.Infrastructure.Persistence.Command
{
    public class KitchenDbContextDesignFactory : IDesignTimeDbContextFactory<KitchenDbContext>
    {
        public KitchenDbContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Kitchen")
                ?? throw new InvalidOperationException("Nenhuma string de conexão encontrada no serviço de Cozinha.");

            var optionsBuilder = new DbContextOptionsBuilder<KitchenDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new KitchenDbContext(optionsBuilder.Options);
        }
    }
}