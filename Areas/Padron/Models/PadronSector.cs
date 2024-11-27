using System;
namespace Sicem_Blazor.Padron.Models {
    public class PadronSector {
        public int Sector {get;set;} = 0;
        public int Activo {get;set;} = 0;
        public int Espera {get;set;} = 0;
        public int BajaTemporal {get;set;} = 0;
        public int BajaDefinitiva {get;set;} = 0;
        public int Congelado {get;set;} = 0;
        public int TotalUsuarios {get;set;} = 0;
        
    }
 }