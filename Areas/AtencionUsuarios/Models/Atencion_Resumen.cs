using System;
using System.Collections.Generic;
using System.Text;
using Sicem_Blazor.Data;

namespace Sicem_Blazor.Models {
    public class Atencion_Resumen {

        public int IdEstatus {get;set;}
        public IEnlace Enlace {get;set;}
        public string NombreEnlace {
            get{
                return Enlace.Nombre;
            }
        }

        public int Total {get;set;} = 0;
        public int Pendiente {get;set;} = 0;
        public int Atendido {get;set;} = 0;
        public int Resuelto  {get;set;} = 0;
        public int Sin_resolver {get;set;} = 0;

        public int Descuentos {get;set;} = 0;
        public decimal Imp_Descuentos {get;set;} = 0m;
        public int Convenios {get;set;} = 0;
        public decimal Imp_Convenios {get;set;} = 0m;

        public IEnumerable<Atencion_Resumen_Genero> Atencion_Generacion { get;set;} = new Atencion_Resumen_Genero[]{};

    }
    public class Atencion_Resumen_Genero {
        public string Id_Genero{get;set;} = "";
        public string Genero{get;set;} = "";

        public int Total {get;set;} = 0;
        public int Pendiente {get;set;} = 0;
        public int Atendido {get;set;} = 0;
        public int Resuelto  {get;set;} = 0;
        public int Sin_resolver {get;set;} = 0;
    }

}
