using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Descuentos.Models{
    public class DescuentosClave {
        public string Cve_Descuento { get; set; }
        public decimal Conc_Sin_Iva { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

    }

}
