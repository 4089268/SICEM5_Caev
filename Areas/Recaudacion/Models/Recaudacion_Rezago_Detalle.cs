using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models{
    public class Recaudacion_Rezago_Detalle {
        public string Cuenta {get;set;}
        public string Usuario {get;set;}
        public string Tarifa {get;set;}
        public int Id_Tarifa {get;set;}
        public int Meses_Adeudo {get;set;}
        public decimal Agua {get;set;}
        public decimal Dren {get;set;}
        public decimal Trat {get;set;}
        public decimal Otros {get;set;}
        public decimal Recar {get;set;}
        public decimal Subtotal {get;set;}
        public decimal Iva {get;set;}
        public decimal Total {get;set;}
    }
}
