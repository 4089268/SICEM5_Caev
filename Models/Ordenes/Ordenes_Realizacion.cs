using System;

namespace Sicem_Blazor.Models{
    public class Ordenes_Realizacion{        
        public int Id {get;set;}
        public string Descripcion{get;set;}
        public int  Ejecutadas {get;set;}
        public int  No_Ejecutadas {get;set;}
        public int  Eje_0_3 {get;set;}
        public int  Eje_4_6 {get;set;}
        public int  Eje_7_9 {get;set;}
        public int  Eje_10 {get;set;}        
        public int  Total {get;set;}        
    }
}