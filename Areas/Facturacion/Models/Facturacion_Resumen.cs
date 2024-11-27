using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Facturacion.Models{
    public class Facturacion_Resumen {
        

        public decimal Fact_Propios{ get; set; }
        public decimal Fact_Otros { get; set; }
        public decimal Recargos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Importe_Total { get; set; }
        
        public int Usuarios { get; set; }
        public int Metros_Fact { get; set; }
        public int Metros_Cons { get; set; }
        

        public Facturacion_Resumen_Hist[] Historial { get; set; }

        public string[] Tarifas { get; set; }

    }

    public class Facturacion_Resumen_Hist{
        public string Periodo {get;set;}
        public decimal Fact_Propios{ get; set; }
        public decimal Fact_Otros { get; set; }
        public decimal Recargos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Importe_Total { get; set; }
        
    }


}
