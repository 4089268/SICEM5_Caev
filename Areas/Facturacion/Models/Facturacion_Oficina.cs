using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Facturacion.Models{

    public class Facturacion_Oficina {
        public int Estatus {get;set;}
        public int Id_Oficina {get;set;}
        public string Oficina {get;set;}


        public int Domestico_Usu {get;set;}
        public decimal Domestico_Fact {get;set;}

        public int Hotelero_Usu {get;set;}
        public decimal Hotelero_Fact {get;set;}

        public int Comercial_Usu {get;set;}
        public decimal Comercial_Fact {get;set;}

        public int Industrial_Usu {get;set;}
        public decimal Industrial_Fact {get;set;}

        public int ServGener_Usu {get;set;}
        public decimal ServGener_Fact {get;set;}

        public decimal Subtotal {get;set;}
        public decimal Iva {get;set;}
        public decimal Total {get;set;}

        public int Usuarios{get;set;}


        public Facturacion_Oficina (){
            Estatus = 0;
            Id_Oficina = 999;
            Oficina = "";
            Domestico_Usu = 0;
            Domestico_Fact = 0m;
            Hotelero_Usu = 0;
            Hotelero_Fact = 0m;
            Comercial_Usu = 0;
            Comercial_Fact = 0m;
            Industrial_Usu = 0;
            Industrial_Fact = 0m;
            ServGener_Usu = 0;
            ServGener_Fact = 0m;
            Subtotal = 0m;
            Iva = 0m;
            Total = 0m;
            Usuarios = 0;

        }
    }
}