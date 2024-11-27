using System;

namespace Sicem_Blazor.Models {
    public class HistTarifaItem {
        public string Oficina { get; set; }
        public int Af { get; set; }
        public int Mf { get; set; }
        public int IdTarifa { get; set; }
        public int IdTipoUsuario { get; set; }
        public string TipoUsuario { get; set; }
        public int Desde { get; set; }
        public int Hasta { get; set; }
        public decimal Costo { get; set; }
        public decimal CostoBase { get; set; }
        public decimal CuotaBase { get; set; }
        public decimal AdicionalM3 { get; set; }
        public DateTime? UltimAct { get; set; }

        public string Mes { get {
            var d = new DateTime(Af , Mf, 1);
            return d.ToString("MMMM").ToUpper();
        } }

    }

}