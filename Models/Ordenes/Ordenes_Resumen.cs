namespace Sicem_Blazor.Models {
    public class Ordenes_Resumen {

        public int Pendi {get;set;}
        public int Eneje {get;set;}
        public int Reali {get;set;}
        public int Cance {get;set;}
        public int Eje {get;set;}
        public int No_eje {get;set;}
        public int Total {get;set;}

        public Ordenes_Resumen_Trabajo[] Trabajos {get;set;}

        public Ordenes_Resumen_Departamento[] Departamentos{get;set;}
        
    }

    public class Ordenes_Resumen_Trabajo{
        public int Id {get;set;}
        public string Descripcion {get;set;}
        public int Pendi {get;set;}
        public int Eneje {get;set;}
        public int Reali {get;set;}
        public int Cance {get;set;}
        public int Eje {get;set;}
        public int No_eje {get;set;}
        public int Total {get;set;}
    }

    public class Ordenes_Resumen_Departamento{
        public int Id {get;set;}
        public string Descripcion {get;set;}
        public int Total {get;set;}
    }

}