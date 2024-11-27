using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicem_Blazor.Recaudacion.Models{
    public class Ingresos_FormasPago {
        public int Orden { get; set; }
        public int Id { get; set; }
        public string Forma_Pago { get; set; }
        public decimal Cobrado{ get; set; }
        public decimal Arqueo{ get; set; }
        public decimal Diferencia { get; set; }
    }
}
