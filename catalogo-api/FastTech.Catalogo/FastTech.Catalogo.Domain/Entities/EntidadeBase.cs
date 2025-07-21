using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTech.Catalogo.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public virtual Guid Id { get; set; }
    }
}
