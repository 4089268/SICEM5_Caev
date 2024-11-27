using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicem_Blazor.Models{
    public class Ordenes_Oficina {
        public int Estatus { get; set; }
        public int IdOficina { get; set; }
        public string Oficina { get; set; }
        public int Pendi { get; set; }
        public int Eneje { get; set; }
        public int Reali { get; set; }
        public int Cance { get; set; }
        public int Eje { get; set; }
        public int No_eje { get; set; }
        public int Total { get; set; }

        public Ordenes_Oficina() {
            Estatus = 0;
            IdOficina = 0;
            Oficina = "";
            Pendi = 0;
            Eneje = 0;
            Reali = 0;
            Cance = 0;
            Eje = 0;
            No_eje = 0;
            Total = 0;
        }

    }
}
