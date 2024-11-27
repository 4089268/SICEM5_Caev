using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Eficiencia.Models{
    public class Eficiencia_Colonia {
        public string Colonia { get; set; }
        public int Id_Colonia { get; set; }
        public int Id_Localidad { get; set; }
        public decimal Facturado { get; set; }
        public double Por_Fact { get; set; }
        public decimal Cobrado { get; set; }
        public double Por_Cobrado { get; set; }
        public decimal Descontado { get; set; }
        public double Por_Desc { get; set; }
        public decimal Anticipado { get; set; }
        public double Por_Ant { get; set; }
        public double Por_Efi { get; set; }
        public double Por_Inef { get; set; }
    }
}
