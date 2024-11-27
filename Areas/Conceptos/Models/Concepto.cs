using System;
using Sicem_Blazor.Models.Entities.Arquos;

namespace Sicem_Blazor.Conceptos.Models {
    public class Concepto {
        public int Id_Concepto {get;set;} = 0;
        public string Descripcion {get;set;} = "";
        public decimal Importe {get;set;} = 0m;
        public bool Credito {get;set;} = false;
        public bool Mostrar {get;set;} = false;
        public int Id_Estatus {get;set;} = 0;
        public string Estatus {get;set;} = "";
        public bool Inactivo {get;set;} = false;
        public bool Costo_Estatico {get;set;} = false;
        public int Id_TipoConcepto {get;set;} = 0;

    }    
}