using FastTech.Catalogo.Domain.Entities;
using FastTech.Catalogo.Domain.Interfaces.Command;
using FastTech.Catalogo.Infrastructure.Persistence.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Infrastructure.Repositories.Command
{
    public class ItemCommandRepository : CommandRepositoryBase<Item>, IItemCommandRepository
    {
        public ItemCommandRepository(CatalogoCommandDbContext context) : base(context) { }
    }
}
