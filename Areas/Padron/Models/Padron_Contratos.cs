using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Padron.Models{
    public class Padron_Contratos {
        public string Id_Contrato {get;set;}
        public DateTime	Fecha_Contratado {get;set;}
        public long Cuenta {get;set;}
        public string Localizacion {get;set;}
        public string Usuario {get;set;}
        public string Tarifa_Contratada {get;set;}
        public string Tarifa_Actual {get;set;}
        public decimal Subtotal {get;set;}
        public decimal Iva {get;set;}
        public decimal Total {get;set;}
        public int Id_Tarifa_Contratada { get; set; }
        public int Id_Tarifa_Actual { get; set; }

    }
}