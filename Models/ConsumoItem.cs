using System;
using System.Collections.Generic;
using System.Text;

namespace Sicem_Blazor.Models {
    public class ConsumoItem {
        private string[]  tmpList = new string[] { "ENE", "FEB", "MAR", "ABR", "MAY", "JUN", "JUL", "AGO", "SEP", "OCT", "NOV", "DIC" };
        public int Id { get; set; }
        public int Id_padron { get; set; }
        public int Mf { get; set; }
        public int Af { get; set; }
        public double Consumo_Act{ get; set; }
        public double Lectura_ant { get; set; }
        public double Lectura_act { get; set; }
        public DateTime Fecha { get; set; }
        public string MesFacturado {
            get { return string.Format("{0} {1}", tmpList[this.Mf-1], Af); }
        }
    }
}
