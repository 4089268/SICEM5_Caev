using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class PagoCentCatGrupo
    {
        public PagoCentCatGrupo()
        {
            OprGrupos = new HashSet<PagoCentOprGrupo>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<PagoCentOprGrupo> OprGrupos { get; set; }
    }
}
