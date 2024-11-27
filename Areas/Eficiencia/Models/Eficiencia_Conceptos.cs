using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Eficiencia.Models{
    public class Eficiencia_Conceptos {
        public string Tipo_Usuario { get; set; }

        public decimal Facturacion { get; set; }
        public decimal Cobrado { get; set; }
        public double Por_Cob { get; set; }
        public decimal Descontado { get; set; }
        public double Por_Des { get; set; }
        public decimal Anticipado { get; set; }
        public double Por_Ant { get; set; }
        public double Por_Efi { get; set; }
        public double Por_Ineficiencia { get; set; }

        public decimal Fac_Agua { get; set; }
        public decimal Fac_Dren { get; set; }
        public decimal Fac_Sane { get; set; }
        public decimal Cob_Agua { get; set; }
        public decimal Cob_Dren { get; set; }
        public decimal Cob_Sane { get; set; }
        public decimal Ant_Agua { get; set; }
        public decimal Ant_Dren { get; set; }
        public decimal Ant_Sane { get; set; }
        public decimal Des_Agua { get; set; }
        public decimal Des_Dren { get; set; }
        public decimal Des_Sane { get; set; }

    }
}
