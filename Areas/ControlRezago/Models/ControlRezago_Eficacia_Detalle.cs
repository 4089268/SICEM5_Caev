using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.ControlRezago.Models{
    public class ControlRezago_Eficacia_Detalle {

        public string Trabajador { get; set; }
        public int Ord_Tot { get; set; }
        public int Ord_efe { get; set; }
        public double Porc_efic_ord { get; set; }
        public decimal Imp_gestionado { get; set; }
        public decimal Imp_cobrado { get; set; }
        public double Porc_efic_cob { get; set; }
    }
}
