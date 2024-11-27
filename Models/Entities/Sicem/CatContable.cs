using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Models
{
    public partial class CatContable
    {
        public decimal? Mayor { get; set; }
        public decimal? Cuenta { get; set; }
        public decimal? Subcuenta { get; set; }
        public string Concepto { get; set; }
        public string CtaContable { get; set; }
        public decimal IdCuenta { get; set; }
        public string IdContable { get; set; }
    }
}
