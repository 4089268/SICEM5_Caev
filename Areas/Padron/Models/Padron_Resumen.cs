using System;
using Sicem_Blazor.Data;
namespace Sicem_Blazor.Padron.Models{
    public class Padron_Resumen {
        public int Estatus { get; set; }
        public IEnlace Enlace {get;set;}
        
        public int Activos_Usuarios {get;set;}
        public decimal Activos_Adeudos {get;set;}
        
        public int Espera_Usuarios {get;set;}
        public decimal Espera_Adeudos {get;set;}

        public int BajaTemp_Usuarios {get;set;}
        public decimal BajaTemp_Adeudos {get;set;}

        public int BajaDef_Usuarios {get;set;}
        public decimal BajaDef_Adeudos {get;set;}

        public int Conge_Usuarios {get;set;}
        public decimal Conge_Adeudos {get;set;}
        
        public int Total_Usuarios {get;set;}
        public decimal Total_Adeudos {get;set;}

        public string FechaModificacion {get;set;}

        public string Oficina { 
            get => Enlace == null?"TOTAL":Enlace.Nombre;
        }

        public int Id {
            get => Enlace==null?0:Enlace.Id;
        }

        public Padron_Resumen() {
            Estatus = 0;
            Activos_Usuarios = 0;
            Activos_Adeudos = 0m;
            Espera_Usuarios = 0;
            Espera_Adeudos = 0m;
            BajaTemp_Usuarios =0;
            BajaTemp_Adeudos = 0m;
            BajaDef_Usuarios = 0;
            BajaDef_Adeudos = 0m;
            Conge_Usuarios = 0;
            Conge_Adeudos = 0m;
            Total_Usuarios = 0;
            Total_Adeudos = 0m;
            FechaModificacion = "";
        }

    }
}