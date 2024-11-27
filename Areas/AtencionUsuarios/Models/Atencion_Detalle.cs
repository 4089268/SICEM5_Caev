using System;

namespace Sicem_Blazor.Models{
    public class Atencion_Detalle{

        public string Folio {get;set;}
        public long Cuenta {get;set;}
        public string Nombre {get;set;}
        public int Id_Colonia {get;set;}
        public string Colonia {get;set;}
        public int Id_Estatus {get;set;}
        public string Estatus {get;set;}
        public int Id_Asunto {get;set;}
        public string Asunto {get;set;}
        public string Descripcion {get;set;}
        public string Resultado {get;set;}
        public DateTime Fecha_Genero {get;set;}
        public string Id_Genero {get;set;}
        public string Genero {get;set;}

    }
}