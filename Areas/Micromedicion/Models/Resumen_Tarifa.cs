using System;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Micromedicion.Models {
    public class Resumen_Tarifa {
        public int Id {get => Enlace == null?999:Enlace.Id ;}
        public string Oficina {get => Enlace==null?"TOTAL":Enlace.Nombre;}
        public IEnlace Enlace {get;set;}
        public string Descripcion {get;set;}
        public int IdTarifa {get;set;}
        public int UsuMedidorFun {get;set;}
        public int UsuMedidorNoFun {get;set;}
        public int UsuConMedidor {get;set;}
        public int UsuSinMedidor {get;set;}
        public int UsuDren {get;set;}
        public int UsuTomas {get;set;}

        public int IdLocalidad {get;set;} = 0;
        public String Localidad {get;set;} = "";

    }
}