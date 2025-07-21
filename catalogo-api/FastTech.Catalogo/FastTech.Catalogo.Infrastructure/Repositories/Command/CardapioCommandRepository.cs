using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Infrastructure.Persistence.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure.Repositories.Command
{
    public class CardapioCommandRepository : CommandRepositoryBase<Cardapio>, ICardapioCommandRepository
    {
        public CardapioCommandRepository(CatalogoCommandDbContext context) : base(context) { }

        public async Task LimparItensESalvarAsync(Guid cardapioId)
        {
            var existentes = await _commandContext
                .Set<Dictionary<string, object>>("CardapioItem")
                .Where(x => EF.Property<Guid>(x, "CardapioId") == cardapioId)
                .ToListAsync();

            _commandContext.RemoveRange(existentes);
            await _commandContext.SaveChangesAsync();
        }
    }
}
