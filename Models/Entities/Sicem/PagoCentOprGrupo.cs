using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class PagoCentOprGrupo
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public int IdOficina { get; set; }
        public int IdLocalidad { get; set; }
        public int Cuenta { get; set; }
        public int IdPadron { get; set; }

        public virtual PagoCentCatGrupo IdGrupoNavigation { get; set; }
        public virtual Ruta IdOficinaNavigation { get; set; }
    }
}
