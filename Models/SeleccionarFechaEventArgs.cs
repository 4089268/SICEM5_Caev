using System;

namespace Sicem_Blazor.Models {
    public class SeleccionarFechaEventArgs {
        public DateTime Fecha1 { get; set; }
        public DateTime Fecha2 { get; set; }
        public int Sector { get; set; }
        public int Subsistema { get; set; }
        public int IdEstatus {get;set;}

        public SeleccionarFechaEventArgs(){
            this.Fecha1 = DateTime.Now;
            this.Fecha2 = DateTime.Now;
            this.Sector = 0;
            this.Subsistema = 0;
            this.IdEstatus = 0;
        }

    }
    
}
