using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class LocalizacionRutum
    {
        public int Id { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int RutaId { get; set; }

        public virtual Ruta Ruta { get; set; }
    }
}
