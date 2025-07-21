using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Autenticacao.Infrastructure.Persistance.Command
{
    public class AutenticacaoCommandDbContextFactory : IDesignTimeDbContextFactory<AutenticacaoCommandDbContext>
    {
        public AutenticacaoCommandDbContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")
                ?? throw new InvalidOperationException("Nenhuma string de conexão encontrada no serviço de cardapio.");

            var optionsBuilder = new DbContextOptionsBuilder<AutenticacaoCommandDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AutenticacaoCommandDbContext(optionsBuilder.Options);
        }
    }
}
