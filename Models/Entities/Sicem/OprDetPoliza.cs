using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class OprDetPoliza
    {
        public decimal IdPoliza { get; set; }
        public decimal? IdCuenta { get; set; }
        public decimal? IdAuxiliar { get; set; }
        public decimal? Cargos { get; set; }
        public decimal? Abonos { get; set; }
        public bool? Recargado { get; set; }
        public string Concepto { get; set; }
        public string Auxiliar { get; set; }
    }
}
