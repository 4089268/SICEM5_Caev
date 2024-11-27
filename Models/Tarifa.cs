using System;
using Sicem_Blazor.Tarifas.Data;

namespace Sicem_Blazor.Models {
    public class Tarifa : ITarifa {
        public int Estatus {get;set;} = 0;
        public int IdOficina {get;set;} = 0;
        public string  Oficina {get;set;} = "";
        
        public int Id_Tarifa{get;set;} = 0;
        public int Id_TipoUsuario {get;set;} = 0;
        public String TipoUsuario {get;set;} = "";
        public int Desde {get;set;} = 0;
        public int Hasta {get;set;} = 0;
        public decimal Costo {get;set;} = 0m;
        public decimal CostoBase {get;set;} = 0m;
        public decimal CuotaBase {get;set;} = 0m;
        public decimal AdicionalM3 {get;set;} = 0m;
       
    }

}