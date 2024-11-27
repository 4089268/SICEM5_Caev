using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Eficiencia.Models{
    public class Eficiencia_Detalle {

        public string Tipo_Usuario { get; set; }
        public decimal Facturacion { get; set; }
        public double Por_Fac { get; set; }
        public decimal Cobrado { get; set; }
        public double Por_Cob { get; set; }
        public decimal Descontado { get; set; }
        public double Por_Des { get; set; }
        public decimal Anticipado { get; set; }
        public double Por_Ant { get; set; }
        public double Por_Efi { get; set; }
    }
}
