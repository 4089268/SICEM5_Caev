using System;
using System.Collections.Generic;
using System.Linq;
using Sicem_Blazor.Data;
using Sicem_Blazor.Data.Contracts;

namespace Sicem_Blazor.ControlRezago.Models {
    public class DetalleRezago {
        public string Estatus {get;set;}
        public string Cuenta {get;set;}
        public string Localizacion {get;set;}
        public string Nombre {get;set;}
        public string Direccion {get;set;}
        public string Tarifa {get;set;}
        public string Colonia {get;set;}
        public int Lec_ant {get;set;}
        public int Lec_act {get;set;}
        public int Consumo {get;set;}
        public string Calculo {get;set;}
        public int Promedio {get;set;}
        public string Medidor {get;set;}
        public int Ma {get;set;}
        public decimal Agua {get;set;}
        public decimal Dren {get;set;}
         public decimal Sane {get;set;}
        public decimal Actu {get;set;}
        public decimal Otros {get;set;}
        public decimal RezAgua {get;set;}
        public decimal RezDren {get;set;}
        public decimal RezTrat {get;set;}
        public decimal Subtotal {get;set;}
        public decimal Iva {get;set;}
        public decimal Total {get; set;}

        public string Sector {
            get {
                var arrs = Localizacion.Split("-");
                return $"{arrs[0]} - {arrs[1]}";
            }
        }
        
        public DetalleRezago(){
            Estatus ="";
            Cuenta ="";
            Localizacion ="";
            Nombre ="";
            Direccion ="";
            Tarifa ="";
            Colonia ="";
            Lec_ant = 0;
            Lec_act =0;
            Consumo =0;
            Calculo ="";
            Promedio = 0;
            Medidor ="";
            Ma = 0;
            Agua = 0m;
            Dren = 0m;
            Sane = 0m;
            Otros = 0m;
            Actu = 0m;
            RezAgua = 0m;
            RezDren = 0m;
            RezTrat = 0m;
            Subtotal = 0m;
            Iva = 0m;
            Total =0m;
        }

    }
}