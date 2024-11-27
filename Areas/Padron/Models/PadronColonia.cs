using System;
namespace Sicem_Blazor.Padron.Models {
    public class PadronColonia {
        public int IdColonia {get;set;} = 0;
        public string Colonia {get;set;} = "";
        public int Activo {get;set;} = 0;
        public int Espera {get;set;} = 0;
        public int BajaTemporal {get;set;} = 0;
        public int BajaDefinitiva {get;set;} = 0;
        public int Congelado {get;set;} = 0;
        public int TotalUsuarios {get;set;} = 0;
    }
 }