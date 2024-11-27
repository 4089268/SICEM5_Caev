using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class StoTarifa
    {
        public StoTarifa()
        {
            StoDetTarifas = new HashSet<StoDetTarifa>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<StoDetTarifa> StoDetTarifas { get; set; }
    }
}
