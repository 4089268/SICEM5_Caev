using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class DetModsOficina
    {
        public int Id { get; set; }
        public int IdModif { get; set; }
        public string Descripcion { get; set; }
        public string ValorAnt { get; set; }
        public string ValorNuevo { get; set; }

        public virtual ModsOficina IdModifNavigation { get; set; }
    }
}
