using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CfgTarifa
    {
        public decimal IdTarifa { get; set; }
        public decimal IdTipoUsuario { get; set; }
        public decimal? Desde { get; set; }
        public decimal? Hasta { get; set; }
        public decimal? Costo { get; set; }
        public decimal? CostoBase { get; set; }
        public bool Inactivo { get; set; }
        public decimal? IdPoblacion { get; set; }
        public DateTime? FechaEdito { get; set; }
        public string IdEdito { get; set; }
        public decimal? CuotaBase { get; set; }
        public decimal? AdicionalM3 { get; set; }
    }
}
