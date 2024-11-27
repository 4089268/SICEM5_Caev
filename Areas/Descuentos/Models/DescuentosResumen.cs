using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Descuentos.Models{
    public class DescuentosResumen {

        public decimal Conc_Con_Iva{ get; set; }
        public decimal Iva { get; set; }
        public decimal Apli_Con_Iva { get; set; }
        public decimal Conc_Sin_Iva { get; set; }
        public decimal Total { get; set; }
        public int Usuarios { get; set; }

        public string[] Conceptos { get; set; }

        public DescuentosResumenItem[] Tarifas { get; set; }

        public DescuentosResumenItem[] Calculos { get; set; }

    }
    public class DescuentosResumenItem {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Total { get; set; }
        public int NTotal { get; set; }
    }

}
