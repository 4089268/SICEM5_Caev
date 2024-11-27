using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models{
    public class Atencion_Group {
        public int Id {get;set;}
        public string Descripcion {get;set;}

        public int Pendiente {get;set;}
        public int Atendido {get;set;}
        public int Resuelto  {get;set;}
        public int Sin_Resolver {get;set;}
        public int Total {get;set;}

    }
}