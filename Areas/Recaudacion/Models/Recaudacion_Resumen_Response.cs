using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models{
    public class Recaudacion_Resumen_Response {
        public decimal Ingresos_propios { get; set; }
        public decimal Ingresos_otros { get; set; }
        public decimal Ingresos_Total { get; set; }
        public decimal Ingresos_Cobrados { get; set; }
        public int Usuarios_propios { get; set; }        
        public int Usuario_otros { get; set; }        
        public int Usuarios_Total { get; set; }

        public string[] Tarifas { get; set; }
        public List<Recaudacion_Resumen_Item> Lista_Ingresos_Conceptos { get; set; }

    }

    public class Recaudacion_Resumen_Item {
        public int Id_concepto { get; set; }
        public string Concepto { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

    }

}
