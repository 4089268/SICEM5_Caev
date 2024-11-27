using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.ControlRezago.Models{
    public class ControlRezago_Eficacia_Resumen {

        public int Id_Trabajo { get; set; }
        public string Trabajo { get; set; }
        public int PENDIENTES { get; set; }
        public int EN_EJECUCION { get; set; }
        public int REAL_EJEC { get; set; }
        public int REAL_NO_EJEC { get; set; }
        public int REALIZADAS { get; set; }
        public int CANCELADAS { get; set; }
        public int TOTALES { get; set; }
    }
}
