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
    public class TipoRefeicaoCommandRepository : CommandRepositoryBase<TipoRefeicao>, ITipoRefeicaoCommandRepository
    {
        public TipoRefeicaoCommandRepository(CatalogoCommandDbContext context) : base(context) { }
    }
}
