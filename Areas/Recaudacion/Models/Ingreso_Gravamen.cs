using System;
using System.Data;
using System.Data.SqlClient;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Recaudacion.Models {

    public class Ingreso_Gravamen {

        public int Id_Concepto {get;set;}
        public string Concepto {get;set;}
        public decimal Importe1 {get;set;}  // No Afectos
        public decimal Importe2 {get;set;}  // Tasa 0%
        public decimal Importe3 {get;set;}  // Tasa 16 %
        public decimal Importe4 {get;set;}  // Iva
        public decimal Total {get;set;}
        public int Usuarios1 {get;set;}
        public int Usuarios2 {get;set;}
        public int Usuarios3 {get;set;}
        public int Usuarios4 {get;set;}
        public int Usuarios {get;set;}
    

    }
}