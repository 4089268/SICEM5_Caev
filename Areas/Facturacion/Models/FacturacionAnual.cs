using System;

namespace Sicem_Blazor.Facturacion.Models {
    public class FacturacionAnual{

        public int Mes  {get;set; } = 0;
        public string Descripcion {get;set;} = "";
        public int Usuarios {get;set;} = 0;
        public decimal SubTotal {get;set;} = 0m;
        public decimal Iva {get;set;} = 0m;
        public decimal Total {get;set;} = 0m;
        public int Volumne {get;set;} = 0;
        public int VolumneFact {get;set;} = 0;

    }
}