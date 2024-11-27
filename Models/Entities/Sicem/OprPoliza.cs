using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class OprPoliza
    {
        public decimal IdPoliza { get; set; }
        public int? IdOficina { get; set; }
        public decimal? Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string IdGenero { get; set; }
        public DateTime? FechaInsert { get; set; }
        public string Firma1 { get; set; }
        public decimal? Año { get; set; }
        public decimal? Mes { get; set; }
        public decimal? Poliza { get; set; }
        public bool? InActivo { get; set; }
        public int? IdSistema { get; set; }
        public string Cheque { get; set; }
    }
}
