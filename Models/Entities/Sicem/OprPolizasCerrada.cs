using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class OprPolizasCerrada
    {
        public decimal Id { get; set; }
        public int? IdOficina { get; set; }
        public decimal? Año { get; set; }
        public decimal? Mes { get; set; }
    }
}
