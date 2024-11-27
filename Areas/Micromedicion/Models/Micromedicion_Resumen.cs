using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Micromedicion.Models {
    public class Micromedicion_Resumen {
        public Micromedicion_Item[] Analisis;
    }

    public class Micromedicion_Item {
        public int Mes { get; set; }
        public string Descripcion_Mes { get; set; }
        public int Reales { get; set; }
        public double Reales_Porc { get; set; }
        public int Promedios { get; set; }
        public double Promedios_Porc { get; set; }
        public int Medidos { get; set; }
        public double Medidos_Porc { get; set; }
        public int Fijos { get; set; }
        public double Fijos_Porc { get; set; }
        public int Total { get; set; }

    }

}
