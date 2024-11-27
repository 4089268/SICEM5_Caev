using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Recaudacion.Models {
    public class Recaudacion_PagosMayores_Response{
        public Recaudacion_PagosMayores[] PagosMayores {get;set;}
        public Recaudacion_PagosMayores_Items[] PagosMayores_Detalle {get;set;}
    }
    public class Recaudacion_PagosMayores {
        public int Id_Padron {get;set;}
        public int Id_Cuenta { get; set; }
        public DateTime Fecha { get; set; }        
        public decimal Subtotal { get; set; } 
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string Sucursal { get; set; }
        public  string Id_Venta { get; set; }
        public string Nombre {get;set;}
        public string Direccion {get;set;}
        public int Id_Publico {get;set;}        

    }

    public class Recaudacion_PagosMayores_Items{
        public string Id_Venta {get;set;}
        public string Concepto {get;set;}
        public decimal Subtotal {get;set;}
        public decimal Iva {get;set;}
        public decimal Total {get;set;}

    }

}
