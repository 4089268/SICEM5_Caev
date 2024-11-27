using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicem_Blazor.Models {
    public class ChartItem {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor1 { get; set; }
        public decimal Valor2 { get; set; }
        public decimal Valor3 { get; set; }
        public decimal Valor4 { get; set; }
        public decimal Valor5 { get; set; }
        public decimal Valor6 { get; set; }
        
        public string AccumulatioText { get; set; }

        public ChartItem() {
            Id = 999;
            Descripcion = "";
            Valor1 = 0m;
            Valor2 = 0m;
            Valor3 = 0m;
            Valor4 = 0m;
            Valor5 = 0m;
            Valor6 = 0m;
            AccumulatioText = "";
        }

    }
}
