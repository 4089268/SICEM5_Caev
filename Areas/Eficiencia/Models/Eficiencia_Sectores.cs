using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Eficiencia.Models{
    public class Eficiencia_Sectores_Resp {
        public Eficiencia_Sectores[] Importes { get; set; }
        public Eficiencia_Sectores[] Metros { get; set; }
        public Eficiencia_Sectores[] Usuarios { get; set; }
    }
    public class Eficiencia_Sectores {
        public string Sector { get; set; }
        public decimal Facturacion { get; set; }
        public double Por_Fact { get; set; }
        public decimal Cobrado { get; set; }
        public double Por_Cobrado { get; set; }
        public decimal Descontado { get; set; }
        public double Por_Desc { get; set; }
        public decimal Anticipado { get; set; }
        public double Por_Ant { get; set; }
        public double Por_Efi { get; set; }
        public double Por_Inefi { get; set; }

    }

}
